using Blogger.Controllers;
using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Options;
using Blogger.Services;
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
    public class PostControllerTests
    {
        private readonly ITestOutputHelper output;

        public PostControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            AppOptionProvider.JwtOptions = new JwtOptions()
            {
                Secret = "My_Secret123134132131232132132132112313"
            };
        }

        public Mock<IPostService> GetService()
        {
            return new Mock<IPostService>();
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

            var controllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { }
            };
            return controllerContext;
        }

        private PostDto GetPostDto()
        {
            return new PostDto()
            {
                Title = "title",
                Body = "body",
                CreatedOn = DateTime.Now
            };
        }


        [Fact]
        public void Success_To_Fetch_A_Post_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Get(It.IsAny<Guid>()))
                .Returns(async (Guid id) =>
                {
                    var postDto = new PostDto();
                    var authorDto = new AuthorDto();
                    postDto.Id = id;
                    postDto.Author = authorDto;
                    return await Task.FromResult(postDto);

                });
            var controller = new PostController(mockService.Object);
            var postId = Guid.NewGuid();
            var actionResult = controller.GetPost(postId).Result as OkObjectResult;

            Assert.NotNull(actionResult);

            var post = actionResult.Value as PostDto;
            Assert.NotNull(post);
            Assert.Equal(postId, post.Id);
        }

        [Fact]
        public void Failed_To_Fetch_An_Invalid_Post_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Get(It.IsAny<Guid>()))
                .Returns(async (Guid id) =>
                {
                    PostDto response = null;
                    return await Task.FromResult(response);

                });
            var controller = new PostController(mockService.Object);
            var postId = Guid.NewGuid();
            var actionResult = controller.GetPost(postId).Result as NotFoundResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Success_To_Create_A_Post_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.Create(It.IsAny<Post>()))
                .Returns(async (Post post) =>
                {
                    post.Id = Guid.NewGuid();
                    return await Task.FromResult(post);

                });
            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var postDto = GetPostDto();
            var actionResult = controller.CreatePost(postDto).Result as CreatedResult;

            Assert.NotNull(actionResult);
            Assert.IsType<string>(actionResult.Location);

            var post = actionResult.Value as Post;
            Assert.NotNull(post);
            Assert.Equal(1, post.AuthorId);
            Assert.IsType<Guid>(post.Id);
            Assert.IsType<DateTime>(post.LastUpdateOn);
            Assert.Equal(postDto.Title, post.Title);
            Assert.Equal(postDto.Body, post.Body);
            Assert.Equal(postDto.CreatedOn, post.CreatedOn);
        }

        [Fact]
        public void Success_To_Update_A_Post_Test()
        {
            var mockService = GetService();
            var lastUpdate = DateTime.MinValue;
            Post oldPost = new Post();
            oldPost.Id = Guid.NewGuid();
            oldPost.Title = "old title";
            oldPost.Body = "old body";
            oldPost.AuthorId = 1;
            oldPost.CreatedOn = DateTime.Now;
            oldPost.LastUpdateOn = lastUpdate;

            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    return await Task.FromResult(oldPost);
                });

            mockService
                .Setup(service => service.Update(It.IsAny<Post>(), It.IsAny<Post>()))
                .Returns(async (Post oldPost, Post newPost) =>
                {
                    oldPost.Title = newPost.Title;
                    oldPost.Body = newPost.Body;
                    oldPost.CreatedOn = newPost.CreatedOn;
                    oldPost.LastUpdateOn = DateTime.Now;
                    return await Task.FromResult(oldPost);
                });



            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim } ;
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var postDto = GetPostDto();
            postDto.Id = oldPost.Id;
            var actionResult = controller.UpdatePost(postDto).Result as OkObjectResult;

            Assert.NotNull(actionResult);

            var post = actionResult.Value as Post;
            Assert.NotNull(post);
            Assert.Equal(1, post.AuthorId);
            Assert.Equal(oldPost.Id, post.Id);
            Assert.NotEqual(lastUpdate, post.LastUpdateOn);
            Assert.Equal(postDto.Title, post.Title);
            Assert.Equal(postDto.Body, post.Body);
            Assert.Equal(postDto.CreatedOn, post.CreatedOn);
        }

        [Fact]
        public void Forbidden_To_Update_A_Post_Of_Another_Author_Test()
        {
            var mockService = GetService();
            Post oldPost = new Post();
            oldPost.Id = Guid.NewGuid();
            oldPost.Title = "old title";
            oldPost.Body = "old body";
            oldPost.AuthorId = 2;
            oldPost.CreatedOn = DateTime.Now;
            oldPost.LastUpdateOn = DateTime.Now;

            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    return await Task.FromResult(oldPost);
                });


            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var postDto = GetPostDto();
            postDto.Id = oldPost.Id;
            var actionResult = controller.UpdatePost(postDto).Result as ForbidResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Failed_To_Update_An_Invalid_Post_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    Post response = null;
                    return await Task.FromResult(response);
                });


            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var postDto = GetPostDto();
            postDto.Id = Guid.NewGuid();
            var actionResult = controller.UpdatePost(postDto).Result as NotFoundResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Failed_To_Delete_An_Invalid_Post_Test()
        {
            var mockService = GetService();
            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    Post response = null;
                    return await Task.FromResult(response);
                });


            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;
            
            var actionResult = controller.DeletePost(Guid.NewGuid()).Result as NotFoundResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Farbidden_To_Delete_A_Post_Of_Another_Author_Test()
        {
            var mockService = GetService();
            Post oldPost = new Post();
            oldPost.Id = Guid.NewGuid();
            oldPost.Title = "old title";
            oldPost.Body = "old body";
            oldPost.AuthorId = 2;
            oldPost.CreatedOn = DateTime.Now;
            oldPost.LastUpdateOn = DateTime.Now;

            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    return await Task.FromResult(oldPost);
                });


            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var actionResult = controller.DeletePost(oldPost.Id).Result as ForbidResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Success_To_Delete_A_Post_Test()
        {
            var mockService = GetService();
            Post oldPost = new Post();
            oldPost.Id = Guid.NewGuid();
            oldPost.Title = "old title";
            oldPost.Body = "old body";
            oldPost.AuthorId = 1;
            oldPost.CreatedOn = DateTime.Now;
            oldPost.LastUpdateOn = DateTime.Now;

            mockService
                .Setup(service => service.GetPostOnly(It.IsAny<Guid>()))
                .Returns(async () =>
                {
                    return await Task.FromResult(oldPost);
                });

            mockService
                .Setup(service => service.Delete(It.IsAny<Post>()))
                .Returns(async (Post post) =>
                {
                    return await Task.FromResult(post);
                });

            var controller = new PostController(mockService.Object);
            controller.ObjectValidator = GetValidator();

            var controllerContext = GetControllerContext();
            Claim claim = new Claim(ClaimTypes.Name, "1");
            List<Claim> claimList = new List<Claim>() { claim };
            ClaimsIdentity identity = new ClaimsIdentity(claimList);
            controllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            controller.ControllerContext = controllerContext;

            var actionResult = controller.DeletePost(oldPost.Id).Result as OkObjectResult;

            Assert.NotNull(actionResult);

            var post = actionResult.Value as Post;
            Assert.Equal(oldPost, post);
        }
    }
}
