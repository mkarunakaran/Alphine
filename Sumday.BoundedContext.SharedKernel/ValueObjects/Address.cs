using System.Collections.Generic;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Address : Masked
    {
        public Address(AddressLine line1, AddressLine line2, City city, State state, PostalCode postalCode, string country = "US", AddressType addressType = null)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new ShouldNotBeEmptyException(nameof(Address.Country));
            }

            this.Line1 = line1;
            this.Line2 = line2;
            this.City = city;
            this.State = state;
            this.Country = country;
            this.PostalCode = postalCode;
            this.AddressType = addressType ?? AddressType.Primary;
        }

        public string Country { get; }

        public string City { get; }

        public string Line1 { get; }

        public string Line2 { get; }

        public State State { get; }

        public PostalCode PostalCode { get; }

        public AddressType AddressType { get; }

        public override string ToMaskedString()
        {
            return string.Format("{0}{0}/{0}{0}/{0}{0}{0}{0}", Mask);
        }

        public override string ToString()
        {
            return $"{this.Line1}\n{this.Line2}\n{this.City}, {this.State} {this.PostalCode}\n{this.Country}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Line1;
            yield return this.Line2;
            yield return this.City;
            yield return this.State;
            yield return this.PostalCode;
            yield return this.Country;
        }
    }
}
