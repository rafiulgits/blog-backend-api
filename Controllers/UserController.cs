using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Extensions;
using Blogger.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<string>> Signup([FromBody] User user)
        {
            var newUser = await userService.Create(user);
            return newUser.GetToken();
        }
    }
}