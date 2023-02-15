using Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.College
{
    public class CollegeBeneficiary : Entity
    {
        public CollegeBeneficiary(string id, PersonalIdentity identity, CollegeBeneficiaryRelationshipType relationshipToOwner, string title)
        {
            this.Id = id;
            this.Identity = identity;
            this.RelationshipToOwner = relationshipToOwner;
            this.Title = title;
        }

        [EntityKey]
        public PersonalIdentity Identity { get; }

        public string Title { get; }

        public string Id { get; }

        public CollegeBeneficiaryRelationshipType RelationshipToOwner { get; }
    }
}
