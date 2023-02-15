using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public abstract class Identity : ValueObject
    {
        protected Identity(Tin tin, Name fullName,  Address address = null, Phone dayPhone = null, Phone eveningPhone = null)
        {
            this.Tin = tin;
            this.FullName = fullName;
            this.Address = address;
            this.DayPhone = dayPhone;

            this.EveningPhone = eveningPhone;
        }

        public Tin Tin { get;  }

        public Name FullName { get; protected set; }

        public Address Address { get;  }

        public Phone DayPhone { get; private set; }

        public Phone EveningPhone { get; private set; }

        public void ChangePhone(Phone dayPhone, Phone eveningPhone = null)
        {
            this.DayPhone = dayPhone;
            if (eveningPhone != null)
            {
                this.EveningPhone = eveningPhone;
            }
        }

        protected static (string FirstName, string LastName) ParseName(string value)
        {
            // If either the first or last name contains a space in it,
            // we store it with two spaces between so that it can be parsed.
            (string FirstName, string LastName) name = (string.Empty, string.Empty);
            var split = value.IndexOf("  "); // Double Space
            if (split > 0)
            {
                name.FirstName = value.Substring(0, split);
                name.LastName = value[(split + 2) ..];
            }
            else
            {
                split = value.IndexOf(' '); // Single Space
                if (split > 0)
                {
                    name.FirstName = value.Substring(0, split);
                    name.LastName = value[(split + 1) ..];
                }
                else
                {
                    name.FirstName = value;
                }
            }

            return name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Tin;
            yield return this.FullName;
            yield return this.Address;
            yield return this.DayPhone;
            yield return this.EveningPhone;
        }
    }
}
