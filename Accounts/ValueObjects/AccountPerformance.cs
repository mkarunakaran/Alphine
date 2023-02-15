using System;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public class AccountPerformance : AccountSummary
    {
        private List<PerformanceSummary> performanceSummaries;

        public AccountPerformance(decimal purchases, decimal redemptions, decimal balance, decimal gains, decimal rateOfReturn, decimal fees, decimal shares, DateTime? asOfPerformanceDate)
        {
            this.Purchases = purchases;
            this.Redemptions = redemptions;
            this.Balance = balance;
            this.Gains = gains;
            this.RateOfReturn = rateOfReturn;
            this.Fees = fees;
            this.Shares = shares;
            this.AsOfPerformanceDate = asOfPerformanceDate;
            this.performanceSummaries = new List<PerformanceSummary>();
        }

        public IReadOnlyList<PerformanceSummary> PerformanceSummary => this.performanceSummaries.AsReadOnly();

        public void SetPerformanceSummary(List<PerformanceSummary> performanceSummaries)
        {
            this.performanceSummaries = performanceSummaries;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.performanceSummaries;
        }
    }
}
