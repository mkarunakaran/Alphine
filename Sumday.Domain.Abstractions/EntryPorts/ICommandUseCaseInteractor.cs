using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface ICommandUseCaseInteractor
    {
        Task<UseCaseExecutionResult> Send<TCommand, TPayLoad>(CommandUseCase<TCommand, TPayLoad> command, CancellationToken cancellationToken)
              where TCommand : ICommandInputPort;
    }
}
