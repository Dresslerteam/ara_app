﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common.CQRS
{
    public interface IQueryHandler<TQuery> where TQuery : IQuery
    {
        void Handle(TQuery command);
    }
}
