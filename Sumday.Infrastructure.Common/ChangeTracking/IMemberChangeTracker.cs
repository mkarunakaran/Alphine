using System;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal interface IMemberChangeTracker : IChangeTracker
    {
        object OriginalValue { get; }

        object CurrentValue { get; }

        string ElementName { get; }

        Type MemberType { get; }
    }
}
