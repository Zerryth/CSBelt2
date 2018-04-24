using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CSBelt2.Models;
using System.Threading.Tasks;

namespace CSBelt2.Controllers
{
    public class UsersController : Controller
    {
        private Belt2Context _context;

        public UsersController(Belt2Context context)
        {
            _context = context;
        }

        [HttpPost, Route("login")]
        public IActionResult Login(LoginViewModel loginUserModel)
        {
            User existingUser = _context.users.SingleOrDefault(u => u.Email == loginUserModel.LoginEmail);

            if (existingUser != null) // email found in DB
            {
                if (existingUser.Password == loginUserModel.LoginPassword)
                {
                    SaveUserInfoToSession(existingUser);
                    return RedirectToAction("Dashboard", "Belt2");
                }
                else
                {
                    loginUserModel.LoginPasswordConfirmation = 1; // out of Range (0,0) => pw match is false
                    TryValidateModel(loginUserModel);
                }
            }
            else
            {
                loginUserModel.EmailInDb = 1; // out of Range (0,0) => Email match is false;
                TryValidateModel(loginUserModel);
            }
            return View("LoginReg", new LogRegModelBundle());
        }

        [HttpGet, Route(""), Route("login")]
        public IActionResult LoginReg()
        {
            return View("LoginReg", new LogRegModelBundle());
        }

        [HttpGet, Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost, Route("register")]
        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                user.UniqueEmail = _context.users.Count(u => u.Email == user.Email);
                TryValidateModel(user);
                if(ModelState.IsValid)
                {
                    SaveUserInfoToSession(user.CreateUser(_context));
                    return RedirectToAction("Dashboard", "Belt2");
                }
            }
            return View("LoginReg", new LogRegModelBundle());
        }

        public void SaveUserInfoToSession(User user)
        {
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("Alias", user.Alias);
        }
    }
}