using Microsoft.EntityFrameworkCore;
using Blogger.Options;

namespace Blogger.Data
{
    public class BloggerContext : DbContext
    {
        public DbSet<Post> Posts {set; get;}
        public DbSet<User> Users {set; get;}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(AppOptionProvider.DbOptions.ToString());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(user => user.Email).IsUnique();
            builder.Entity<User>().HasIndex(user => user.BlogName).IsUnique();
        }
    }
}