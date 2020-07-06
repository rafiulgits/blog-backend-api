using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] PostDto postDto)
        {
            var post = postDto.GetPersistentObject();
            if(TryValidateModel(post))
            {
                post.AuthorId = HttpContext.GetUserId();
                var result = await postService.Create(post);
                string refUrl = $"{HttpContext.GetCurrentRequestUrl()}/{result.Id.ToString()}";
                return Created(refUrl, result);
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPost(Guid id)
        {
            var result = await postService.Get(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("page")]
        [HttpGet("page/{skip}")]
        [HttpGet("page/{skip}/{top}")]
        public async Task<ActionResult> GetPaginate(int skip=0, int top=20)
        {
            var queryOrder = Request.Query["order"].ToString();
            if(!String.IsNullOrEmpty(queryOrder))
            {
                queryOrder = queryOrder.ToLower();
                if(queryOrder == "desc")
                {
                    var resultWithOrder = await postService.GetPage(skip, top, true);
                    return Ok(resultWithOrder);
                }
            }
            var result = await postService.GetPage(skip, top, false);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllPosts()
        {
            string filter = Request.Query["filter"];
            var result = await postService.GetAll(filter);
            return Ok(result);
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
            var oldPost = await postService.GetPostOnly(post.Id);
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
        public async Task<ActionResult> DeletePost(Guid id)
        {
            var post = await postService.GetPostOnly(id);
            if(post == null)
            {
                return NotFound();
            }
            if(post.AuthorId != HttpContext.GetUserId())
            {
                return Forbid();
            };
            var result = await postService.Delete(post);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("blog/{name}")]
        public async Task<ActionResult> GetPostsByBlog(string name)
        {
            var result = await postService.GetPostsByBlog(name);
            if(result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound();
            
        }
    }
}