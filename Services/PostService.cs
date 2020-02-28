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

        public async Task<Post> Update(Post post, Guid id)
        {
            Post fetchedPost = await PostRepo.Get(id);
            if(fetchedPost == null)
            {
                return null;
            }
            fetchedPost.Title = post.Title;
            fetchedPost.Body = post.Body;
            fetchedPost.CreatedOn = post.CreatedOn;
            fetchedPost.LastUpdateOn = DateTime.Now;
            return await PostRepo.Update(fetchedPost);
        }

        public async Task<Post> Delete(Guid id)
        {
            var post = await PostRepo.Get(id);
            if(post == null)
            {
                return null;
            }
            return await PostRepo.Delete(post);
        }

        public async Task<List<Post>> GetAll()
        {
           return await PostRepo.GetQueryableHandler()
                                .AsQueryable()
                                .Where(post => true)
                                .ToListAsync();
        }

        public async Task<List<Post>> GetPage(int page, int pageSize=10, bool descOrder=false)
        {
            if(page <=0)
            {
                page = 1;
            }

            var resource = PostRepo.GetQueryableHandler();
            int cursorPoint = (page-1)*pageSize;
            if(descOrder)
            {
                return await resource.OrderByDescending(post => post.CreatedOn)
                                     .Skip(cursorPoint)
                                     .Take(pageSize)
                                     .ToListAsync();
            }
            return await resource.Skip(cursorPoint)
                                 .Take(pageSize)
                                 .ToListAsync();
        }
    }
}