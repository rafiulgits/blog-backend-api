using Microsoft.EntityFrameworkCore;

namespace Blogger.Data
{
    public class BloggerContext : DbContext
    {
        public DbSet<Post> Posts {set; get;}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=localhost;Initial Catalog=Blogger;Integrated Security=True");
        }
    }
}