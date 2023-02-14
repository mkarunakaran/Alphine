using Newtonsoft.Json;
using System;
using System.Linq;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class Address
    {
        public const string DefaultCountryCode = "US";


        public Address()
        {
            this.CountryCode = DefaultCountryCode;
        }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }
      

        public int? AgeOfAdult { get; set; }

        public string PostalCode { get; set; }
       
        public string CountryCode { get; set; }

        public DateTime? LastModified { get; set; }

        public int HashCode { get; set; }


        public override string ToString()
        {
            return $"{this.Line1}\n{this.Line2}\n{this.City}, {this.StateCode} {this.PostalCode}\n{this.CountryCode}";
        }
    }
}
