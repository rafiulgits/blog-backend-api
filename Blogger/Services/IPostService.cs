using Blogger.Data;
using Blogger.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IPostService
    {
        Task<PostDto> Get(Guid id);
        Task<Post> GetPostOnly(Guid id);
        Task<Post> Create(Post post);
        Task<Post> Update(Post oldPost, Post newPost);
        Task<Post> Delete(Post entity);
        Task<List<PostDto>> GetAll(string filter);
        Task<List<PostDto>> GetPage(int skip, int top, bool descOrder);
        Task<List<PostDto>> GetPostsByBlog(string name);
    }
}
