using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class SubObjectChangeTrackerTemplate : IMemberChangeTrackerTemplate
    {
        private readonly MemberInfo map;
        private readonly object originalValue;
        private readonly List<IMemberChangeTrackerTemplate> trackerTemplates;

        public SubObjectChangeTrackerTemplate(object aggregate, MemberInfo map)
        {
            this.map = map;

            this.originalValue = aggregate?.GetPropValue(this.map.Name);

            this.trackerTemplates = GetChangeTrackerTemplates(this.map.GetMemberInfoType(), this.originalValue);
        }

        public IMemberChangeTracker ToChangeTracker(object aggregate)
        {
            return new SubObjectChangeTracker(aggregate, this.map, this.originalValue, this.trackerTemplates.ToTrackers(aggregate.GetPropValue(this.map.Name)).ToList());
        }

        private static List<IMemberChangeTrackerTemplate> GetChangeTrackerTemplates(Type memberType, object entity)
        {
            var allMembers = memberType.GetMembers();
            return allMembers.Where(map => map is PropertyInfo)
               .Select(map => ChangeTrackerProvider.GetTrackerTemplateForMember(entity, map))
               .ToList();
        }
    }
}
