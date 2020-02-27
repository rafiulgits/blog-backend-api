using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Services;

namespace Blogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {

        private readonly PostService postService;

        public PostController(PostService postService)
        {
            this.postService = postService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] PostDto post)
        {
            if(post.IsValid())
            {
                var result = await postService.Create(post.GetPersistentObject());
                return Created(result.Id.ToString(), result);
            }
            return BadRequest(post.Errors);
        }
    }
}