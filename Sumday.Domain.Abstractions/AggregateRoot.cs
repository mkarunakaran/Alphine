using System.Collections.Generic;
using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.Domain.Abstractions
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();

        public IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents;

        public void ClearDomainEvents()
        {
            this.domainEvents.Clear();
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            this.domainEvents.Add(domainEvent);
        }
    }
}
