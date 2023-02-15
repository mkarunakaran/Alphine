using Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able
{
    public class AbleBeneficiary : Entity
    {
        public AbleBeneficiary(string id, PersonalIdentity identity, Eligibility eligibility, string title)
        {
            this.Id = id;
            this.Identity = identity;
            this.Eligibility = eligibility;
            this.Title = title;
        }

        [EntityKey]
        public PersonalIdentity Identity { get; }

        public string Id { get; }

        public string Title { get; }

        public Eligibility Eligibility { get; }
    }
}
