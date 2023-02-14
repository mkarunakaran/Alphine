using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class CommandUseCaseInteractor : ICommandUseCaseInteractor
    {
        private readonly IMediator mediator;

        public CommandUseCaseInteractor(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<UseCaseExecutionResult> Send<TCommand, TPayLoad>(CommandUseCase<TCommand, TPayLoad> command, CancellationToken cancellationToken)
            where TCommand : ICommandInputPort
        {
            return this.mediator.Send(command, cancellationToken);
        }
    }
}
