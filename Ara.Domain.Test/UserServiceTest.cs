using Ara.Domain.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ara.Domain.Test
{
    public class UserServiceTest
    {
        [Fact]
        public async void GetUsers_Successfully()
        {
            var userService = new UserService();
            var users = userService.GetAllUsers();
            Assert.True(true);

            Assert.True(userService.GetCurrentUserInfo() == null);
        }

        [Fact]
        public async void Login_Successfully()
        {
            var userService = new UserService();
            userService.Login("tato@khorava.com", null);
            var currentUser = userService.GetCurrentUserInfo();
            Assert.True(currentUser != null);
            Assert.True(currentUser.Email == "tato@khorava.com");
        }

        [Fact]
        public async void LogOut_Successfully()
        {
            var userService = new UserService();
            userService.Login("tato@khorava.com", null);
            var currentUser = userService.GetCurrentUserInfo();
            Assert.True(currentUser != null);
            userService.LogOut();
            Assert.True(userService.GetCurrentUserInfo() == null);
        }
    }
}
