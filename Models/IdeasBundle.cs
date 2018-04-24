using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CSBelt2.Models
{
    public class IdeasBundle
    {
        public Idea IdeaModel { get; set; }
        public List<Idea> AllIdeas { get; set; }
        public IdeasBundle CreateIdeasBundle(Belt2Context _context)
        {
            IdeasBundle newIdeasBundle = new IdeasBundle
            {
                IdeaModel = new Idea(),
                AllIdeas = _context.ideas.Include(i => i.UsersWhoLiked).ThenInclude(m => m.User).ToList()
            };
            return newIdeasBundle;
        }
    }

}