using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients.Dtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
