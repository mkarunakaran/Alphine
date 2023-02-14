using System.Collections.Generic;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal interface IObjectChangeTracker
    {
        IReadOnlyCollection<IMemberChangeTracker> MemberTrackers { get; }
    }
}
