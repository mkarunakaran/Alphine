using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class ManagerOrSelfInformationProfile : ValueObject
    {
        public ManagerOrSelfInformationProfile(PersonalIdentity managerOrSelfIdentity, InformationProfile managerOrSelfnformationProfile)
        {
            this.ManagerOrSelfIdentity = managerOrSelfIdentity;
            this.ManagerOrSelfnformationProfile = managerOrSelfnformationProfile;
        }

        public PersonalIdentity ManagerOrSelfIdentity { get; }

        public InformationProfile ManagerOrSelfnformationProfile { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.ManagerOrSelfIdentity;
            yield return this.ManagerOrSelfnformationProfile;
        }
    }
}
