using System.Reflection;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class MemberChangeTrackerTemplate : IMemberChangeTrackerTemplate
    {
        private readonly MemberInfo map;
        private readonly object originalValue;

        public MemberChangeTrackerTemplate(object aggregate, MemberInfo map)
        {
            this.map = map;
            this.originalValue = aggregate?.GetPropValue(map.Name);
        }

        public IMemberChangeTracker ToChangeTracker(object aggregate)
        {
            return new MemberChangeTracker(aggregate, this.map, this.originalValue);
        }
    }
}
