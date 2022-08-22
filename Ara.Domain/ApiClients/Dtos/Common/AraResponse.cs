using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApiClients.Dtos.Common
{
    public class AraResponse<T>
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Error { get; set; }
        public T Payload { get; set; }
    }
}
