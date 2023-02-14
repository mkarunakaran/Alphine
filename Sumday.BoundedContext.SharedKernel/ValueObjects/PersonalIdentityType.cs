using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class PersonalIdentityType : Enumeration<PersonalIdentityType, int>
    {
        public PersonalIdentityType(int type, string value)
          : base(type, value)
        {
        }

        public static PersonalIdentityType Adult => new PersonalIdentityType(0, "Adult");

        public static PersonalIdentityType Minor => new PersonalIdentityType(1, "Minor");

        public static implicit operator PersonalIdentityType(int value) => FromValue(value);

        public static implicit operator string(PersonalIdentityType identityType) => identityType.Name;
    }
}
