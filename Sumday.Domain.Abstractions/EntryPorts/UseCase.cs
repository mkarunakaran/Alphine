using MediatR;
using System;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public abstract class UseCase : IRequest<UseCaseExecutionResult>
    {
        protected UseCase()
        {
            this.UseCaseId = Guid.NewGuid().ToString();
            this.UseCaseSentOn = DateTime.UtcNow;
        }

        public string UseCaseId { get; }

        public DateTime UseCaseSentOn { get; }
    }
}
