using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.Entities
{
    public class Employment : ValueObject
    {
        private readonly List<SourceOfIncome> sourcesOfIncome;

        public Employment(string primaryAccountNumber, string occupation, EmploymentStatus status)
        {
            this.PrimaryAccountNumber = primaryAccountNumber;
            this.Occupation = occupation;
            this.Status = status;
            this.sourcesOfIncome = new List<SourceOfIncome>();
        }

        public string PrimaryAccountNumber { get; }

        public string Occupation { get; }

        public EmploymentStatus Status { get; }

        public IReadOnlyList<SourceOfIncome> SourcesOfIncome => this.sourcesOfIncome.AsReadOnly();

        public string SourcesOfIncomeOther { get; private set; }

        public void AddSourceOfIncome(SourceOfIncome sourceOfIncome)
        {
            this.sourcesOfIncome.Add(sourceOfIncome);
        }

        public void SetSourcesOfIncomeOther(string sourcesOfIncomeOther)
        {
            this.SourcesOfIncomeOther = sourcesOfIncomeOther;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.PrimaryAccountNumber;
            yield return this.SourcesOfIncomeOther;
            yield return this.Occupation;
            yield return this.Status;
        }
    }
}
