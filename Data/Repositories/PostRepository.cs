using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data
{
    public class PostRepository : IRepository<Guid, Post>
    {

        private readonly BloggerContext Context;

        public PostRepository(BloggerContext context)
        {
            Context = context;
        }
        
        public async Task<Post> Get(Guid id)
        {
            return await Context.FindAsync<Post>(id);
        }

        public async Task<Post> Add(Post entity)
        {
            var result = await Context.AddAsync<Post>(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Post> Update(Post entity)
        {
            var result =  Context.Update<Post>(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }


        public async Task<Post> Delete(Post entity)
        {
            var result = Context.Remove<Post>(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public IQueryable<Post> GetQueryableHandler()
        {
            return Context.Posts.AsQueryable();
        }
    }
}