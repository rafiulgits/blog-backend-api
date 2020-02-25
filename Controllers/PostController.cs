using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Blogger.Data;
using Blogger.Dto;
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
        public IActionResult CreatePost([FromBody] PostCreateDto formData)
        {
            if(formData.IsValid())
            {
                var result = _PostService.PostRepo.Add(formData.GetObject());
                return Created(result.Id.ToString(), result);
            }
            return BadRequest("requested body is no valid");
        }
    }
}