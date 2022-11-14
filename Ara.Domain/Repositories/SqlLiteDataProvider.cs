using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ara.Domain.Repositories
{
    public interface IDataProvider
    {
        IDbConnection Db { get; set; }
    }
    
}
