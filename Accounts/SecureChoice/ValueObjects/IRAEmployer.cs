using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public class IRAEmployer : ValueObject
    {
        public IRAEmployer(string recordId, Name name, string id, EnrollmentStatus enrollmentStatus, int deferralRate, bool accountAutoEscalation)
        {
            this.Id = id;
            this.RecordId = recordId;
            this.Name = name;
            this.EnrollmentStatus = enrollmentStatus;
            this.DeferralRate = deferralRate;
            this.AccountAutoEscalation = accountAutoEscalation;
        }

        public string RecordId { get; }

        public string Name { get;  }

        public string Id { get; set; }

        public EnrollmentStatus EnrollmentStatus { get; }

        public int DeferralRate { get; }

        public bool AccountAutoEscalation { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.RecordId;
            yield return this.Name;
            yield return this.Id;
            yield return this.EnrollmentStatus;
            yield return this.DeferralRate;
            yield return this.AccountAutoEscalation;
        }
    }
}
