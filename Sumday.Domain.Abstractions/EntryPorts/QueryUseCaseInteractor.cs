using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class QueryUseCaseInteractor : IQueryUseCaseInteractor
    {
        private readonly IMediator mediator;

        public QueryUseCaseInteractor(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<UseCaseExecutionResult> Send<TQuery, TPayLoad>(QueryUseCase<TQuery, TPayLoad> query, CancellationToken cancellationToken)
             where TQuery : IQueryInputPort
        {
            return this.mediator.Send(query, cancellationToken);
        }
    }
}
