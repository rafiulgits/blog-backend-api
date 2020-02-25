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

        private readonly PostService _PostService;

        public PostController(PostService postService)
        {
            _PostService = postService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] PostDto formData)
        {
            if(formData.IsValid())
            {
                var result = await _PostService.Create(formData.GetObject());
                return Created(result.Id.ToString(), result);
            }
            return BadRequest(formData.Errors);
        }
    }
}