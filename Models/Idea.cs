using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CSBelt2.Models
{
    public class Idea: BaseEntity
    {
        [Key]
        public int IdeaId { get; set; }

        [Required]
        public string Description { get; set; }
        public int PosterId { get; set; }
        public User Poster { get; set; }

        public List<LikesMap> UsersWhoLiked { get; set; }

        public Idea()
        {
            UsersWhoLiked = new List<LikesMap>();
        }
    }
}