using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly BloggerContext Context;

        public UserRepository(BloggerContext context)
        {
            Context = context;
        }

        public async Task<User> Get(int id)
        {
            return await Context.FindAsync<User>(id);
        }

        public async Task<User> Add(User entity)
        {
            var result = await Context.Users.AddAsync(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> Update(User entity)
        {
            var result = Context.Update<User>(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> Delete(User entity)
        {
            var result = Context.Remove<User>(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public IQueryable<User> GetQueryableHandler()
        {
            return Context.Users.AsQueryable();
        }
    }
}
