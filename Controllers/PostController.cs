using Microsoft.AspNetCore.Mvc;
using Blogger.Data;
using Blogger.Dto;

namespace Blogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {

        private readonly BloggerContext DB;

        public PostController()
        {
            DB = new BloggerContext();
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] PostCreateDto formData)
        {
            if(formData.IsValid())
            {
                var result = DB.Add(formData.GetObject());
                DB.SaveChanges();
                return Created($"{result.Entity.Id}", result.Entity);
            }
            return BadRequest("requested body is no valid");
        }
    }
}