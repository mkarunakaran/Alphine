using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public sealed class EmploymentStatus : Enumeration<EmploymentStatus, string>
    {
        public EmploymentStatus(string value, string name)
            : base(value, name)
        {
        }

        public static EmploymentStatus Employed => new EmploymentStatus("EM", "Employed");

        public static EmploymentStatus SelfEmployed => new EmploymentStatus("SE", "SelfEmployed");

        public static EmploymentStatus NotEmployedRetired => new EmploymentStatus("NE", "NotEmployedRetired");

        public static implicit operator EmploymentStatus(string value) => FromValue(value) ?? FromName(value);

        public static implicit operator string(EmploymentStatus employmentStatus) => employmentStatus.Value;
    }
}
