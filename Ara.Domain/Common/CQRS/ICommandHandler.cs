using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common.CQRS
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
