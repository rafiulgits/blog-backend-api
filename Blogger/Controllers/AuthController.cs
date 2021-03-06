﻿using Blogger.Data.Dto;
using Blogger.Extensions;
using Blogger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blogger.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] AuthDto authDto)
        {
            var result = authService.Authenticate(authDto.Email, authDto.Password);
            if (result.IsValid)
            {
                var response = new TokenDto() { Bearer = result.Data.GetToken() };
                return Ok(response);
            }

            return Unauthorized(result.Error);
        }
    }
}
