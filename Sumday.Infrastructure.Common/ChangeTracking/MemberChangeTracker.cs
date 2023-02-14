using System;
using System.Reflection;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class MemberChangeTracker : IMemberChangeTracker
    {
        private readonly object aggregate;
        private readonly MemberInfo memberMap;

        public MemberChangeTracker(object aggregate, MemberInfo memberMap, object originalValue)
        {
            this.aggregate = aggregate;
            this.memberMap = memberMap;
            this.OriginalValue = originalValue;
        }

        public object OriginalValue { get; }

        public object CurrentValue => this.aggregate.GetPropValue(this.ElementName);

        public string ElementName => this.memberMap.Name;

        public virtual bool HasChange => !this.OriginalValue?.Equals(this.CurrentValue) ?? this.CurrentValue != null;

        public Type MemberType => this.memberMap.GetMemberInfoType();
    }
}
