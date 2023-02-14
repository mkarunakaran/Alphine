using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface IQueryUseCaseHandler<TQuery, TPayLoad> : IRequestHandler<QueryUseCase<TQuery, TPayLoad>, UseCaseExecutionResult>
     where TQuery : IQueryInputPort
    {
    }
}
