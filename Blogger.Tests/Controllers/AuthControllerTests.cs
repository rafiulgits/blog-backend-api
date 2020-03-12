using Moq;
using System;
using Xunit;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Services;
using Blogger.Controllers;
using Blogger.Util;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Blogger.Options;
using System.Collections.Generic;

namespace Blogger.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly ITestOutputHelper output;

        public AuthControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            AppOptionProvider.JwtOptions = new JwtOptions() 
            {
                Secret = "My_Secret123134132131232132132132112313"
            };
        }

        private Mock<IAuthService> GetService()
        {
            return new Mock<IAuthService>();
        }

        [Fact]
        public void Login_Success_Test()
        {
            Mock<IAuthService> mockService = GetService();

            mockService
                .Setup(service => service.Authenticate(It.IsAny<String>(), It.IsAny<String>()))
                .Returns((string email, string password) =>
                {
                    var response = new DataDependentResult<User>();
                    response.Data = new User() { Id = 1 };
                    response.IsValid = true;
                    response.Error = null;
                    return response;
                });

            AuthController controller = new AuthController(mockService.Object);
            var request = new AuthDto(){ Email = "rafi@mail.com", Password = "12345678" };
            var actionResult = controller.Login(request) as OkObjectResult;

            Assert.NotNull(actionResult);

            var tokenDto = actionResult.Value as TokenDto;
            Assert.NotNull(tokenDto);
            Assert.IsType<string>(tokenDto.Bearer);
        }

        [Fact]
        public void Login_Failed_By_Invalid_Email_Test()
        {
            Mock<IAuthService> mockService = GetService();

            mockService
                .Setup(service => service.Authenticate(It.IsAny<String>(), It.IsAny<String>()))
                .Returns((string email, string password) =>
                {
                    var response = new DataDependentResult<User>();
                    response.Data = null;
                    response.IsValid = false;
                    response.Error = new ErrorDto().Append("Email", "no user found with this email address");
                    return response;
                });

            AuthController controller = new AuthController(mockService.Object);
            var request = new AuthDto() { Email = "rafi@mail.com", Password = "12345678" };
            var actionResult = controller.Login(request) as UnauthorizedObjectResult;

            Assert.NotNull(actionResult);

            var errorDto = actionResult.Value as ErrorDto;
            Assert.NotNull(errorDto);
        }

        [Fact]
        public void Login_Failed_By_Invalid_Password_Test()
        {
            Mock<IAuthService> mockService = GetService();

            mockService
                .Setup(service => service.Authenticate(It.IsAny<String>(), It.IsAny<String>()))
                .Returns((string email, string password) =>
                {
                    var response = new DataDependentResult<User>();
                    response.Data = null;
                    response.IsValid = false;
                    response.Error = new ErrorDto().Append("Password", "incorrect password");
                    return response;
                });

            AuthController controller = new AuthController(mockService.Object);
            var request = new AuthDto() { Email = "rafi@mail.com", Password = "12345678" };
            var actionResult = controller.Login(request) as UnauthorizedObjectResult;

            Assert.NotNull(actionResult);

            var errorDto = actionResult.Value as ErrorDto;
            Assert.NotNull(errorDto);
        }
    }
}
