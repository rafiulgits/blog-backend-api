using Blogger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IPostService
    {
        Task<Post> Get(Guid id);
        Task<Post> Create(Post post);
        Task<Post> Update(Post oldPost, Post newPost);
        Task<Post> Delete(Post entity);
        Task<List<Post>> GetAll();
        Task<List<Post>> GetPage(int skip, int top, bool descOrder);
    }
}
