using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sumday.Domain.Abstractions;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal static class ChangeTrackerProvider
    {
        public static IMemberChangeTrackerTemplate GetTrackerTemplateForMember(object aggregate, MemberInfo map)
        {
            if (map.GetMemberInfoType().IsSubclassOf(typeof(Entity)))
            {
                return new SubObjectChangeTrackerTemplate(aggregate, map);
            }

            return new MemberChangeTrackerTemplate(aggregate, map);
        }

        public static IEnumerable<IMemberChangeTracker> ToTrackers(
            this IEnumerable<IMemberChangeTrackerTemplate> templates,
            object aggregate)
        {
            if (aggregate == null)
            {
                return System.Array.Empty<IMemberChangeTracker>();
            }

            return templates.Select(t => t.ToChangeTracker(aggregate));
        }
    }
}
