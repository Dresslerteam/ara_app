using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients.Interfaces
{
    internal interface IAuthClient
    {
        AraResponse<TokenResponseDto> Login(string username, string password);
        void LogOut();
        void GetCurrentUserInfo();
    }
}
