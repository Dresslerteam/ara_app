using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients.Interfaces
{
    internal interface IAuthClient
    {
        UserDto Login(string email, string password);
        void LogOut();
        UserDto GetCurrentUserInfo();
        List<UserDto> GetAllUsers();
    }
}
