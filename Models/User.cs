using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace CSBelt2.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<LikesMap> LikedPosts;

        public User()
        {
            LikedPosts = new List<LikesMap>();
        }
    }
}