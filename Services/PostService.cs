using Blogger.Data;

namespace Blogger.Services
{
    public class PostService
    {
        public BloggerContext Context;
        public PostRepository PostRepo;


        public PostService(BloggerContext bloggerContext)
        {
            Context = bloggerContext;
            PostRepo = new PostRepository(Context);
        }
    }
}