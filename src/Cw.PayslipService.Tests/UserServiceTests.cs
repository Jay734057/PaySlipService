using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cw.PayslipService.Tests
{
    public class UserServiceTests : IClassFixture<TestFixture>
    {
        private readonly IUserService _userService;

        public UserServiceTests(TestFixture fixture)
        {
            _userService = fixture.UserService;
        }

        [Fact]
        public void GetUserById_ValidUserId()
        {
            var result = _userService.GetUserById(TestFixture.TEST_USER_ID_1);
            Assert.IsType<User>(result);
            Assert.Equal(TestFixture.TEST_USER_ID_1, result.Id);
        }

        [Fact]
        public void GetUserById_InvalidUserId()
        {
            var result = _userService.GetUserById(TestFixture.TEST_USER_ID_2);
            Assert.Null(result);
        }

        [Fact]
        public void ValidateCredentials_ValidUsernamePassword()
        {
            var result = _userService.ValidateCredentials(TestFixture.TEST_USERNAME_1, TestFixture.TEST_PASSWORD_1);
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void ValidateCredentials_InvalidUsernamePassword()
        {
            var result = _userService.ValidateCredentials(TestFixture.TEST_USERNAME_2, TestFixture.TEST_PASSWORD_2);
            Assert.Null(result);
        }
    }
}
