using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
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
            if(post.IsValid(DtoTypes.RequestType.Create))
            {
                var result = await postService.Create(post.GetPersistentObject());
                string refUrl = $"{HttpContext.Request.GetDisplayUrl()}/{result.Id.ToString()}";
                return Created(refUrl, result);
            }
            return BadRequest(post.Error);
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

        [HttpPut]
        public async Task<ActionResult<Post>> UpdatePost(PostDto post)
        {
            if(!post.IsValid(DtoTypes.RequestType.Update))
            {
                return BadRequest(post.Error);
            }

            var result = await postService.Update(post.GetPersistentObject(), post.Id);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(Guid id)
        {
            var result = await postService.Delete(id);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}