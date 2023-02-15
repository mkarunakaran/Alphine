using Sumday.BoundedContext.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class EntityOrTrustIdentity : Identity
    {
        public EntityOrTrustIdentity(Ein ein, Name fullName, DateTime? dateOfTrust, Address address, Phone dayPhone, Phone eveningPhone)
            : base(ein, fullName, address, dayPhone, eveningPhone)
        {
            this.DateOfTrust = dateOfTrust.HasValue ? dateOfTrust.Value.Date : null;
        }

        public Ein Ein => this.Tin as Ein;

        public DateTime? DateOfTrust { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.DateOfTrust;
        }
    }
}
