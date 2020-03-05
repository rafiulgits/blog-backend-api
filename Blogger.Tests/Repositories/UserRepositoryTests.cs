using Blogger.Data;
using Blogger.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Blogger.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly ITestOutputHelper output;
        public UserRepositoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }


        private DbContextOptions GetOptions()
        {
            return new DbContextOptionsBuilder<BloggerContext>()
                        .UseInMemoryDatabase(databaseName: "blogger_test_db").Options;
        }

        [Fact]
        public void Save_And_Fetch_Test()
        {
            var options = GetOptions();
            using (var context = new BloggerContext(options))
            {
                output.WriteLine(context.Users.GetType().ToString());
                var repo = new UserRepository(context);
                var user = new User()
                {
                    FirstName = "Rafiul",
                    LastName = "Islam",
                    Email = "rafi@mail.com",
                    Password = "123124141",
                    BlogName = "rafiulblog"
                };
                User createdUser = repo.Add(user).Result;
                Assert.Equal(user, createdUser);

                User fetchedUser = repo.Get(1).Result;
                Assert.Equal(user, fetchedUser);
            }

        }
    }
}
