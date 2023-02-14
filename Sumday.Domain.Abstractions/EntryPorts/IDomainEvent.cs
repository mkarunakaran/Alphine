using System;
using MediatR;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn => DateTime.UtcNow;
    }
}
