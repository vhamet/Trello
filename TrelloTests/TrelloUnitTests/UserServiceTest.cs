using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;

using Trello.Models;

namespace TrelloUnitTests
{
    public class UserServiceTest
    {
        [Fact]
        public void Create_saves_a_user_via_context()
        {
            var mockSet = new Mock<DbSet<User>>();

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(m => m.tblUser).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);
            service.Create(new User() { Username = "User A" });
            
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void ValidateUserCredentials_should_return_user_when_credentials_valid()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var user = service.ValidateUserCredentials("User1", "1234");

            Assert.True(user != null);
            Assert.True(user.UserId == 1);
        }

        [Fact]
        public void ValidateUserCredentials_should_return_null_when_credentials_invalid()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var user = service.ValidateUserCredentials("User1", "123");

            Assert.True(user == null);
        }

        [Fact]
        public void CheckUserNameTaken_should_return_true_when_username_taken()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var taken = service.CheckUserNameTaken("User1");

            Assert.True(taken);
        }

        [Fact]
        public void CheckUserNameTaken_should_return_false_when_username_available()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var taken = service.CheckUserNameTaken("User");

            Assert.False(taken);
        }

        [Fact]
        public void CheckEmailTaken_should_return_true_when_email_taken()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var taken = service.CheckEmailTaken("user1@trello.com");

            Assert.True(taken);
        }

        [Fact]
        public void CheckEmailTaken_should_return_false_when_email_available()
        {
            var data = TestUtils.GetUserData();
            var mockContext = TestUtils.InitUserMoqContext(data);
            var service = new UserService(mockContext.Object);
            var taken = service.CheckEmailTaken("user@trello.com");

            Assert.False(taken);
        }
    }
}
