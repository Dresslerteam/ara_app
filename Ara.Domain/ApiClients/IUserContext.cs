using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients
{
    public interface IUserContext
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }

    }
}
