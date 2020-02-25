using System;
using System.Collections.Generic;
using System.Linq;

namespace Blogger.Data
{
    public class PostRepository : IRepository<Guid, Post>
    {

        private readonly BloggerContext Context;

        public PostRepository(BloggerContext context)
        {
            Context = context;
        }
        
        public Post Get(Guid id)
        {
            return Context.Find<Post>(id);
        }

        public List<Post> GetAll()
        {
            return Context.Posts.ToList();
        }

        public Post Add(Post entity)
        {
            var result = Context.Add<Post>(entity);
            Context.SaveChanges();
            return result.Entity;
        }

        public Post Update(Post entity)
        {
            var result =  Context.Update<Post>(entity);
            return result.Entity;
        }


        public Post Delete(Guid id)
        {
            var post = Get(id);
            var result = Context.Remove<Post>(post);
            return result.Entity;
        }
    }
}