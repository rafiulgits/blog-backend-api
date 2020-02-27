using Microsoft.EntityFrameworkCore;
using Blogger.Options;

namespace Blogger.Data
{
    public class BloggerContext : DbContext
    {
        public DbSet<Post> Posts {set; get;}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(AppOptionProvider.DbOptions.ToString());
        }
    }
}