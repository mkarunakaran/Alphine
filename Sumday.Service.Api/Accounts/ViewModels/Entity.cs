using System;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class Entity
    {
        public AccountType Type { get; set; }

        public string Name { get; set; }

        public Cip Cip { get; set; }

        public Address Address { get; set; }

        public string Phone { get; set; }

        public string AltPhone { get; set; }

        public DateTime CreatedDate { get; set; }

        public Tin Tin { get; set; }
    }
}
