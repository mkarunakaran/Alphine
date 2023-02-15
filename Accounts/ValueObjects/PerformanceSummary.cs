using System;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public sealed class PerformanceSummary : AccountSummary
    {
        public PerformanceSummary(string fundId, decimal purchases, decimal redemptions, decimal balance, decimal gains, decimal rateOfReturn, decimal fees, decimal shares, DateTime? asOfPerformanceDate)
        {
            this.Purchases = purchases;
            this.Redemptions = redemptions;
            this.Balance = balance;
            this.Gains = gains;
            this.RateOfReturn = rateOfReturn;
            this.Fees = fees;
            this.Shares = shares;
            this.AsOfPerformanceDate = asOfPerformanceDate;
            this.FundId = fundId;
        }

        public string FundId { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.FundId;
        }
    }
}
