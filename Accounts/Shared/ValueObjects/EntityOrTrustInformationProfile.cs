using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class EntityOrTrustInformationProfile : ValueObject
    {
        public EntityOrTrustInformationProfile(EntityOrTrustIdentity entityOrTrustIdentity, InformationProfile entityProfile)
        {
            this.EntityOrTrustIdentity = entityOrTrustIdentity;
            this.EntityProfile = entityProfile;
        }

        public EntityOrTrustIdentity EntityOrTrustIdentity { get; }

        public InformationProfile EntityProfile { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.EntityOrTrustIdentity;
            yield return this.EntityProfile;
        }
    }
}
