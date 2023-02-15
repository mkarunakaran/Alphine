using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class AbleAccountOwner : AccountOwner
    {
        public AbleAccountOwner(PersonalIdentity identity, Name shortName, AbleAccountType ableAccountType)
        {
            this.Identity = identity;
            this.AbleAccountType = ableAccountType;
            this.ShortName = shortName;
        }

        public override PersonalIdentity Identity { get; }

        public AbleAccountType AbleAccountType { get; }

        public AbleAccountOwnerRelationshipType RelationshipToBeneficiary { get; private set; }

        public AbleAccountOwnerType AbleAccountOwnerType => this.AbleAccountType;

        public void SetAbleAccountOwnerRelationshipType(AbleAccountOwnerRelationshipType relationshipToBeneficiary)
        {
            this.RelationshipToBeneficiary = relationshipToBeneficiary;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Identity;
            yield return this.AbleAccountType;
            yield return this.RelationshipToBeneficiary;
        }
    }
}
