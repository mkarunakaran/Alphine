using System;
using System.Collections.Generic;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public abstract class AccountSummary : ValueObject
    {
        protected AccountSummary()
        {
        }

        public decimal Purchases { get; protected set; }

        public decimal Redemptions { get; protected set; }

        public decimal Balance { get; protected set; }

        public decimal Gains { get; protected set; }

        public decimal RateOfReturn { get; protected set; }

        public decimal Fees { get; protected set; }

        public decimal Shares { get; protected set; }

        public DateTime? AsOfPerformanceDate { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Purchases;
            yield return this.Redemptions;
            yield return this.Balance;
            yield return this.Gains;
            yield return this.RateOfReturn;
            yield return this.Fees;
            yield return this.Shares;
            yield return this.AsOfPerformanceDate;
        }
    }
}
