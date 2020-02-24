using Microsoft.AspNetCore.Mvc;
using Blogger.Data;

namespace Blogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreatePost()
        {
            var DB = new BloggerContext();
            var result = DB.Add(new Post()
            {
                Title = "Hello World",
                Body = "This is my first blog post",
                CreatedOn = System.DateTime.Now
            });
            DB.SaveChanges();
            return Created($"{result.Entity.Id}", result.Entity);
        }
    }
}