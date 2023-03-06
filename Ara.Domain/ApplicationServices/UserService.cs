using Ara.Domain.ApiClients;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara.Domain.ApplicationServices
{
    public class UserService
    {
        private readonly IAuthClient _authClient;
        public UserService()
        {
            _authClient = new AuthClient();
        }

        public UserDto GetCurrentUserInfo()
        {
            return _authClient.GetCurrentUserInfo();
        }

        public UserDto Login(string email, string password = null)
        {
            return _authClient.Login(email, password);
        }

        public void LogOut()
        {
            _authClient.LogOut();
        }

        public List<UserDto> GetAllUsers()
        {
            return _authClient.GetAllUsers();
        }
    }
}
