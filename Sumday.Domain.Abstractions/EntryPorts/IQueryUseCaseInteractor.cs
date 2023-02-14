using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface IQueryUseCaseInteractor
    {
        Task<UseCaseExecutionResult> Send<TQuery, TPayLoad>(QueryUseCase<TQuery, TPayLoad> query, CancellationToken cancellationToken)
             where TQuery : IQueryInputPort;
    }
}
