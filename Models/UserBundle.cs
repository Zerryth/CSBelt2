using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CSBelt2.Models
{
    public class UserBundle
    {
        public User UserModel { get; set; }
        public List<LikesMap> AllLikes { get; set; }
        // public IdeasBundle CreateIdeasBundle(Belt2Context _context)
        // {
        //     IdeasBundle newIdeasBundle = new IdeasBundle
        //     {
        //         IdeaModel = new Idea(),
        //         AllIdeas = _context.ideas.Include(i => i.UsersWhoLiked).ThenInclude(m => m.User).ToList()
        //     };
        //     return newIdeasBundle;
        // }
    }

}