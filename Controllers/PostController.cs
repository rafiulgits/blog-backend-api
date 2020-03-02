using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Services;
using Microsoft.AspNetCore.Authorization;
using Blogger.Extensions;

namespace Blogger.Controllers
{
    [Authorize]
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
                post.AuthorId = HttpContext.GetUserId();
                var result = await postService.Create(post.GetPersistentObject());
                string refUrl = $"{HttpContext.Request.GetDisplayUrl()}/{result.Id.ToString()}";
                return Created(refUrl, result);
            }
            return BadRequest(post.Error);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            return await postService.GetAll();
        }

        [HttpPut]
        public async Task<ActionResult<Post>> UpdatePost([FromBody]PostDto postDto)
        {
            var post = await postService.Get(postDto.Id);
            if (post.AuthorId != HttpContext.GetUserId())
            {
                return Unauthorized();
            }
            if (!postDto.IsValid(DtoTypes.RequestType.Update))
            {
                return BadRequest(postDto.Error);
            }

            var result = await postService.Update(postDto.GetPersistentObject(), post.Id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(Guid id)
        {
            var post = await postService.Get(id);
            if(post.AuthorId != HttpContext.GetUserId())
            {
                return Unauthorized();
            }
            var result = await postService.Delete(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}