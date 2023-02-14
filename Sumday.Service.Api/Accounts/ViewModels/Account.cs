using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class Account
    {
        public static readonly string[] UgmaStateCodes = { "SC", "VT", "VI", "GU" };

        [JsonIgnore]
        private AccountType accountType;

        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime? ConvertedDate { get; set; }

        public DateTime? AsOfPerformanceDate { get; set; }

        public string AccountNumber { get; set; }

        public AccountType Type
        {
            get => (this.accountType == AccountType.Utma) && Account.UgmaStateCodes.Contains(this.Beneficiary?.Address?.StateCode)
                ? AccountType.Ugma
                : this.accountType;

            set => this.accountType = value;
        }

        public AccountStatus Status { get; set; }

        public bool IsSetup { get; set; }

        public AlrStatus? AlrStatus { get; set; }

        public MinorType MinorType { get; set; }

        public string Title { get; set; }

        public string PlanId { get; set; }

        public decimal Goal { get; set; }

        public AbleEligibility AbleEligibility { get; set; }

        public string CustomerId { get; set; }

        public Person Owner { get; set; }

        public Person SecondaryOwner { get; set; }

        public Entity Entity { get; set; }

        public Person Beneficiary { get; set; }

        public Person Successor { get; set; }

        public Address MailingAddress { get; set; }

        public AccountRollover Rollover { get; set; }

        public decimal TotalBalance { get; set; }

        public Person Manager { get; set; }
    }
}
