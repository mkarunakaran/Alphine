using Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice
{
    public class IRABeneficiary : ValueObject
    {
        public IRABeneficiary(string id, Identity identity, IRABeneficiaryType iRABeneficiaryType, decimal percentDesignated, IRABeneficiaryRelationshipType relationshipToOwner)
        {
            this.Id = id;
            this.Identity = identity;
            this.RelationshipToOwner = relationshipToOwner;
            this.PercentDesignated = percentDesignated;
            this.IRABeneficiaryType = iRABeneficiaryType;
         }

        public Identity Identity { get; }

        public string Id { get; }

        public IRABeneficiaryType IRABeneficiaryType { get; }

        public decimal PercentDesignated { get; }

        public IRABeneficiaryRelationshipType RelationshipToOwner { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.IRABeneficiaryType;
            yield return this.Identity;
            yield return this.PercentDesignated;
            yield return this.RelationshipToOwner;
        }
    }
}
