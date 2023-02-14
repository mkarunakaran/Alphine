using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class AggregateChangeTracker<TAggregate> : IChangeTracker, IObjectChangeTracker
        where TAggregate : AggregateRoot
    {
        private readonly TAggregate aggregate;
        private readonly Collection<IMemberChangeTrackerTemplate> memberChangeTrackers;

        public AggregateChangeTracker(TAggregate aggregate)
        {
            this.aggregate = aggregate;
            this.memberChangeTrackers = new Collection<IMemberChangeTrackerTemplate>(GetChangeTrackers(aggregate));
        }

        public bool HasChange => this.MemberTrackers.Any(t => t.HasChange);

        public IReadOnlyCollection<IMemberChangeTracker> MemberTrackers => Array.AsReadOnly(this.memberChangeTrackers.ToTrackers(this.aggregate).ToArray());

        private static List<IMemberChangeTrackerTemplate> GetChangeTrackers(TAggregate aggregate)
        {
            var aggregateType = aggregate.GetType();
            var allMembers = aggregateType.GetMembers();
            return allMembers.Where(map => map is PropertyInfo)
                .Select(map => ChangeTrackerProvider.GetTrackerTemplateForMember(aggregate, map))
                .ToList();
        }
    }
}
