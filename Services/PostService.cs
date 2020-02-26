using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blogger.Data;

namespace Blogger.Services
{
    public class PostService
    {
        public PostRepository PostRepo;

        public PostService(PostRepository postRepository)
        {
            PostRepo = postRepository;
        }

        public async Task<Post> Create(Post post)
        {
            return await PostRepo.Add(post);
        }

        public async Task<Post> Get(Guid id)
        {
            return await PostRepo.Get(id);
        }

        public async Task<Post> Update(Post post)
        {
            return await PostRepo.Update(post);
        }

        public async Task<Post> Delete(Guid id)
        {
            var post = await PostRepo.Get(id);
            return await PostRepo.Delete(post);
        }

        public async Task<List<Post>> GetAll()
        {
            var handler =  PostRepo.GetQueryableHandler();
            return await handler.AsQueryable()
                .Where(post => true).ToListAsync();
        }
    }
}