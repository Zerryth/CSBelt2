using Microsoft.EntityFrameworkCore;

namespace CSBelt2.Models
{
    public class Belt2Context: DbContext
    {
        public Belt2Context(DbContextOptions<Belt2Context> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Idea> ideas { get; set; }
        public DbSet<LikesMap> likesmap { get; set; }
    }
}