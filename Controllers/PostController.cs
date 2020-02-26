using System;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            var result = await postService.Get(id);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("page/{number}")]
        public async Task<ActionResult<List<Post>>> GetPaginate(int number)
        {
            var queryOrder = Request.Query["order"].ToString();
            if(!String.IsNullOrEmpty(queryOrder))
            {
                queryOrder = queryOrder.ToLower();
                if(queryOrder == "desc")
                {
                    return await postService.GetPage(number, 10 ,true);
                }
            }
            return await postService.GetPage(number);
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            return await postService.GetAll();
        }
    }
}