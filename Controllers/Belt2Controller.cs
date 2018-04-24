using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CSBelt2.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CSBelt2.Controllers
{
    public class Belt2Controller : Controller
    {
        private Belt2Context _context;   

        public Belt2Controller(Belt2Context context)
        {
            _context = context;
        }

        [HttpGet, Route("brightideas")]
        public IActionResult Dashboard()
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                IdeasBundle IdeasFormsBundle = new IdeasBundle
                {
                    IdeaModel = new Idea(),
                    AllIdeas = _context.ideas.Include(idea => idea.Poster).Include(i => i.UsersWhoLiked).ThenInclude(m => m.User).OrderByDescending(idea => idea.UsersWhoLiked.Count).ToList()
                };
                
                return View("Dashboard", IdeasFormsBundle);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpPost, Route("brightideas")]
        public IActionResult CreateIdea(IdeasBundle model)
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                int userId = (int)TempData["UserId"];
                if(ModelState.IsValid)
                {
                    User user = _context.users.SingleOrDefault(u => u.UserId == userId);
                    model.IdeaModel.Poster = user;
                    model.IdeaModel.PosterId = userId;
                    _context.ideas.Add(model.IdeaModel);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                model.AllIdeas = _context.ideas.Include(idea => idea.Poster).Include(i => i.UsersWhoLiked).ThenInclude(m => m.User).OrderByDescending(idea => idea.UsersWhoLiked.Count).ToList();
                return View("Dashboard", model);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpGet, Route("delete/{ideaId}")]
        public IActionResult Delete(int ideaId)
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                Idea idea = _context.ideas.SingleOrDefault(i => i.IdeaId == ideaId);
                if (idea.PosterId == (int)TempData["UserId"])
                {
                    // CASCADE property on Idea in db
                    _context.ideas.Remove(idea);
                    _context.SaveChanges();
                }
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Login", "Users");
        }
        public void GetUserFromSession()
        {
            TempData["UserId"] = (int?)HttpContext.Session.GetInt32("UserId");
            TempData["UserName"] = HttpContext.Session.GetString("UserName");
            TempData["Alias"] = HttpContext.Session.GetString("Alias");
        }

        public bool IsLoggedIn()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            return (UserId != null); // evaluates the boolean like an if statement
        }

        [HttpGet, Route("like/{ideaId}")]
        public IActionResult Like(int ideaId)
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                int userId = (int)TempData["UserId"];
                User user = _context.users.SingleOrDefault(u => u.UserId == userId);
                // LikesMap like = _context.likesmap.Where(m => m.IdeaId == ideaId).SingleOrDefault(map => map.UserId == userId);
                _context.likesmap.Add(new LikesMap{ IdeaId = ideaId, UserId = userId });
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet, Route("brightideas/{ideaId}")]
        public IActionResult LikeStatus(int ideaId)
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                var distinctUsersWhoLiked = new Dictionary<string, User>();
                Idea idea = _context.ideas.Include(idear => idear.Poster).Include(i => i.UsersWhoLiked).ThenInclude(m => m.User).Distinct().SingleOrDefault(idear => idear.IdeaId == ideaId);

                foreach(var user in idea.UsersWhoLiked)
                {
                    if(!distinctUsersWhoLiked.ContainsKey(user.User.Name))
                    {
                        distinctUsersWhoLiked.Add(user.User.Name, user.User);
                    }
                }
                ViewBag.DistinctUsers = distinctUsersWhoLiked;
                return View("LikeStatus", idea);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpGet, Route("users/{userId}")]
        public IActionResult Profile(int userId)
        {
            if(IsLoggedIn())
            {
                GetUserFromSession();
                // var user = _context.users.Include(l => l.LikedPosts).SingleOrDefault(u => u.UserId == (int)TempData["UserId"]);
                var map = _context.likesmap.Where(m => m.UserId == userId).ToList();
                var postsCount = _context.ideas.Where(i => i.PosterId == userId).ToList().Count;
                TempData["PostCount"] = postsCount;

                UserBundle UserProfileBundle = new UserBundle()
                {
                    UserModel = _context.users.SingleOrDefault(u => u.UserId == userId),
                    AllLikes = map
                };
                return View("Profile", UserProfileBundle);
            }
            return RedirectToAction("Login", "Users");
        }
    }
}