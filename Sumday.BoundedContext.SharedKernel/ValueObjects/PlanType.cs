using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class PlanType : Enumeration<PlanType, string>
    {
        public PlanType(string type, string value)
          : base(type, value)
        {
        }

        public static PlanType Able => new PlanType("able", "Able");

        public static PlanType CollegeSavings => new PlanType("collegeSavings", "CollegeSavings");

        public static PlanType SecureChoice => new PlanType("secureChoice", "SecureChoice");

        public static implicit operator PlanType(string value) => FromValue(value) ?? FromName(value);

        public static implicit operator string(PlanType planType) => planType.Value;
    }
}
