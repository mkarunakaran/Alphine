using System;
using MediatR;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class CommandUseCase<TCommand, TPayLoad> : UseCase
          where TCommand : ICommandInputPort
    {
        public CommandUseCase(TCommand query, ICommandOutputPort<TPayLoad> outputPort)
        {
            this.Command = query;
            this.OutputPort = outputPort;
        }

        public ICommandOutputPort<TPayLoad> OutputPort { get; }

        public TCommand Command { get; }
    }
}
