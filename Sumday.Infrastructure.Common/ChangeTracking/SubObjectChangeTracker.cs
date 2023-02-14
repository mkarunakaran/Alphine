using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class SubObjectChangeTracker : MemberChangeTracker, IObjectChangeTracker
    {
        private readonly List<IMemberChangeTracker> memberTrackers;

        public SubObjectChangeTracker(object aggregate, MemberInfo memberMap, object originalValue, List<IMemberChangeTracker> memberTrackers)
            : base(aggregate, memberMap, originalValue)
        {
            this.memberTrackers = memberTrackers;
        }

        public IReadOnlyCollection<IMemberChangeTracker> MemberTrackers => Array.AsReadOnly(this.memberTrackers.ToArray());

        public override bool HasChange
        {
            get
            {
                if (this.CurrentValue == null)
                {
                    return this.OriginalValue != null;
                }

                return this.MemberTrackers.Any(t => t.HasChange);
            }
        }
    }
}
