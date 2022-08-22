using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Dtos.Common;
using Ara.Domain.ApiClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients
{
    public class AuthClient : IAuthClient
    {
        public void GetCurrentUserInfo()
        {
            throw new NotImplementedException();
        }

        public AraResponse<TokenResponseDto> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
