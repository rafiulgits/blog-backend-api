using Blogger.Controllers;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Options;
using Blogger.Services;
using Blogger.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blogger.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly ITestOutputHelper output;
        public UserControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            AppOptionProvider.JwtOptions = new JwtOptions()
            {
                Secret = "My_Secret123134132131232132132132112313"
            };
        }

        public Mock<IUserService> GetService()
        {
            return new Mock<IUserService>();
        }

        private UserDto GetUserDto()
        {
            return new UserDto()
            {
                Email = "rafi@mail.com",
                Password = "123456789",
                FirstName = "Rafiul",
                LastName = "Islam",
                BlogName = "rafiulblog"
            };
        }

        private IObjectModelValidator GetValidator()
        {
            /*
             * Reference https://github.com/aspnet/Mvc/issues/3586#issuecomment-291458257
             */
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            return objectValidator.Object;
        }

        private ControllerContext GetControllerContext()
        {

            HttpContext httpContext = new DefaultHttpContext();

            var controllerContext =  new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(){}
            };
            return controllerContext;            
        }

        [Fact]
        public void Failed_To_Create_User_By_Existing_Email_Test()
        {
            var mockService = GetService();
            string errorMessage = "user already exists with this email address";
            mockService
                .Setup(service => service.Create(It.IsAny<User>()))
                .Returns(async (User user) => 
                {
                    var response = new DataDependentResult<User>();
                    response.Data = null;
                    response.IsValid = false;
                    response.Error = new ErrorDto().Append("Email", errorMessage);
                    return await Task.FromResult(response);
                });

            var controller = new UserController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var userDto = GetUserDto();
            var actionResult =  controller.Create(userDto).Result as BadRequestObjectResult;

            Assert.NotNull(actionResult);

            var errorDto = actionResult.Value as ErrorDto;
            Assert.NotNull(errorDto);
            Assert.IsType<List<string>>(errorDto["Email"]);
            Assert.Equal(errorMessage, errorDto["Email"][0]);
        }

        [Fact]
        public void Failed_To_Create_User_By_Existing_BlogName_Test()
        {
            var mockService = GetService();
            string errorMessage = "user already exists with this blog name";
            mockService
                .Setup(service => service.Create(It.IsAny<User>()))
                .Returns(async (User user) =>
                {
                    var response = new DataDependentResult<User>();
                    response.Data = null;
                    response.IsValid = false;
                    response.Error = new ErrorDto().Append("BlogName", errorMessage);
                    return await Task.FromResult(response);
                });

            var controller = new UserController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var userDto = GetUserDto();
            var actionResult = controller.Create(userDto).Result as BadRequestObjectResult;

            Assert.NotNull(actionResult);

            var errorDto = actionResult.Value as ErrorDto;
            Assert.NotNull(errorDto);
            Assert.IsType<List<string>>(errorDto["BlogName"]);
            Assert.Equal(errorMessage, errorDto["BlogName"][0]);
        }

        [Fact]
        public void Success_To_Create_User_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Create(It.IsAny<User>()))
                .Returns(async (User user) =>
                {
                    var response = new DataDependentResult<User>();
                    response.Data = new User() { Id = 1 };
                    response.IsValid = true;
                    response.Error = null;
                    return await Task.FromResult(response);
                });

            var controller = new UserController(mockService.Object);
            controller.ObjectValidator = GetValidator();
            ControllerContext controllerContext = GetControllerContext();
            controllerContext.HttpContext.Request.Path = "api/User/";
            controller.ControllerContext = controllerContext;

            var userDto = GetUserDto();
            var actionResult = controller.Create(userDto).Result as CreatedResult;

            Assert.NotNull(actionResult);
            output.WriteLine(actionResult.Location);
            Assert.IsType<string>(actionResult.Location);

            var tokenDto = actionResult.Value as TokenDto;
            Assert.NotNull(tokenDto);
            Assert.IsType<string>(tokenDto.Bearer);
        }

        [Fact]
        public void Failed_To_Access_Profile_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Get(It.IsAny<string>()))
                .Returns(async (string id) =>
                {
                    User response = null;
                    return await Task.FromResult(response);
                });

            var controller = new UserController(mockService.Object);
            controller.ObjectValidator = GetValidator();
            ControllerContext controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);
            controller.ControllerContext = controllerContext;

            var actionResult = controller.Profile().Result as NotFoundResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Success_To_Access_Profile_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Get(It.IsAny<string>()))
                .Returns(async (string id) =>
                {
                    User response = new User() { Id = 1, Email="rafi@mail.com", FirstName="Rafiul" };
                    return await Task.FromResult(response);
                });

            var controller = new UserController(mockService.Object);
            controller.ObjectValidator = GetValidator();
            ControllerContext controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);
            controller.ControllerContext = controllerContext;

            var actionResult = controller.Profile().Result as OkObjectResult;
            Assert.NotNull(actionResult);
        }
    }
}
