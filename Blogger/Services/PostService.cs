using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blogger.Data;
using Blogger.Extensions;

namespace Blogger.Services
{
    public class PostService : IPostService
    {
        public IPostRepository PostRepo;

        public PostService(IPostRepository postRepository)
        {
            PostRepo = postRepository;
        }

        public async Task<Post> Create(Post post)
        {
            post.Id = Guid.Empty;
            post.LastUpdateOn = DateTime.Now;
            return await PostRepo.Add(post);
        }

        public async Task<Post> Get(Guid id)
        {
            return await PostRepo.Get(id);
        }

        public async Task<Post> Update(Post olderPost, Post updatedPost)
        {
            olderPost.Title = updatedPost.Title;
            olderPost.Body = updatedPost.Body;
            olderPost.CreatedOn = updatedPost.CreatedOn;
            olderPost.LastUpdateOn = DateTime.Now;
            return await PostRepo.Update(olderPost);
        }

        public async Task<Post> Delete(Post post)
        {
            return await PostRepo.Delete(post);
        }

        public async Task<List<Post>> GetAll(string filter)
        {
            if(!String.IsNullOrEmpty(filter))
            {
                return await PostRepo
                    .GetQueryableHandler()
                    .Include("Author")
                    .Where(post => post.Title.Contains(filter) || 
                                   post.Body.Contains(filter) || 
                                   post.Author.BlogName.Contains(filter))
                    .ToListAsync();
            }
            return await PostRepo.GetQueryableHandler()
                                 .Include("Author")
                                 .Where(post => true)
                                 .ToListAsync();
        }

        public async Task<List<Post>> GetPage(int skip, int top, bool descOrder)
        {
            if(skip <0)
            {
                skip = 0;
            }
            if(top < 0)
            {
                top = 20;
            }

            var resource = PostRepo.GetQueryableHandler();
            if(descOrder)
            {
                return await resource.OrderByDescending(post => post.CreatedOn)
                                     .Include("Author")
                                     .Skip(skip)
                                     .Take(top)
                                     .ToListAsync();
            }
            return await resource.Include("Author")
                                 .Skip(skip)
                                 .Take(top)
                                 .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByBlog(string name)
        {
            name = name.ToLower();
            var resource = PostRepo.GetQueryableHandler();
            return await resource.Include("Author")
                                 .Where(post => post.Author.BlogName == name)
                                 .ToListAsync();
        }
    }
}