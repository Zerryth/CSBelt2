using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSBelt2.Models
{
    public class LikesMap : BaseEntity
    {
        [Key]
        public int MapId { get; set; }

        [ForeignKey("Idea")]
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}