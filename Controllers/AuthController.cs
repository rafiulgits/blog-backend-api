using Blogger.Data.Dto;
using Blogger.Extensions;
using Blogger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blogger.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] AuthDto authDto)
        {
            var result = authService.Authenticate(authDto);
            if(result != null)
            {
                var response = new TokenDto() { Bearer = result.GetToken() };
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
