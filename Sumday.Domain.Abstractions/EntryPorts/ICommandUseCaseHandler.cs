using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface ICommandUseCaseHandler<TCommand, TPayLoad> : IRequestHandler<CommandUseCase<TCommand, TPayLoad>, UseCaseExecutionResult>
        where TCommand : ICommandInputPort
    {
    }
}
