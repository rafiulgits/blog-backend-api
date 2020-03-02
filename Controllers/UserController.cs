using System.Threading.Tasks;
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
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserDto user)
        {
            if(user.IsValid(DtoTypes.RequestType.Create))
            {
                var result =  await userService.Create(user.GetPersistentObject());
                var response = new TokenDto() { Bearer = result.GetToken() };
                return Ok(response);
            }
            return BadRequest(user.Error);
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