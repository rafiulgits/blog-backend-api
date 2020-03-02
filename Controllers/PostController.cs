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
        public async Task<ActionResult<Post>> CreatePost([FromBody] PostDto postDto)
        {
            var post = postDto.GetPersistentObject();
            if(TryValidateModel(post))
            {
                post.AuthorId = HttpContext.GetUserId();
                var result = await postService.Create(post);
                string refUrl = $"{HttpContext.Request.GetDisplayUrl()}/{result.Id.ToString()}";
                return Created(refUrl, result);
            }
            return BadRequest(ModelState);
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
        public async Task<ActionResult> UpdatePost([FromBody]PostDto postDto)
        {
            if(postDto.Id == Guid.Empty)
            {
                var error = new ErrorDto().Append("Id", "this field Id is required to update a post object");
                return BadRequest(error);
            }
            var post = postDto.GetPersistentObject();
            var oldPost = await postService.Get(post.Id);
            if(oldPost == null)
            {
                return NotFound();
            }
            if(oldPost.AuthorId != HttpContext.GetUserId())
            {
                return Forbid();
            }
            if(TryValidateModel(post))
            {
                var result = await postService.Update(oldPost, post);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(Guid id)
        {
            var post = await postService.Get(id);
            if(post == null)
            {
                return NotFound();
            }
            if(post.AuthorId != HttpContext.GetUserId())
            {
                return Forbid();
            };
            var result = await postService.Delete(post);
            return Ok(post);
        }
    }
}