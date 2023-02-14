using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class Person
    {
        [Masked]
        public string Ssn { get; set; }
       

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }


        [Masked]
        public DateTime? BirthDate { get; set; }
       

        public bool? IsAdult { get; set; }
      

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        [Masked]
        public Address Address { get; set; }

        [Masked]
        public string Phone { get; set; }

        [Masked]
        public string AltPhone { get; set; }

        public Cip Cip { get; set; }

        public Relationship? Relationship { get; set; }
       
    }
}
