using System.Threading.Tasks;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Extensions;
using Blogger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserDto userDto)
        {
            var user = userDto.GetPersistentObject();
            if(TryValidateModel(user))
            {
                var result = await userService.Create(user);
                if(result.IsValid)
                {
                    var response = new TokenDto() { Bearer = result.Data.GetToken() };
                    return Ok(response);
                }
                return BadRequest(result.Error);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            string id = HttpContext.User.Identity.Name;
            var result = await userService.Get(id);
            if (result != null)
            {
                result.Password = null;
                return Ok(result);
            }
            return NotFound();
        }

    }
}