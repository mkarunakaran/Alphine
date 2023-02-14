using System.Collections.Generic;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class PlanId : ValueObject
    {
        public PlanId(string value, string name, string shortName, PlanType type)
        {
            this.Value = value;
            this.Name = name;
            this.ShortName = shortName;
            this.Type = type;
        }

        public string Value { get; }

        public string Name { get; }

        public string ShortName { get; }

        public PlanType Type { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
            yield return this.Name;
            yield return this.ShortName;
            yield return this.Type;
        }
    }
}
