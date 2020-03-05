using System;

namespace Blogger.Data
{
    public interface IPostRepository : IRepository<Guid, Post>
    {
    }
}
