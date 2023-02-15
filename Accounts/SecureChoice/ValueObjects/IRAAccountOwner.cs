using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public class IRAAccountOwner : AccountOwner
    {
        public IRAAccountOwner(PersonalIdentity identity, Name shortName, IRAAccountType iRAAccountType)
        {
            this.Identity = identity;
            this.ShortName = shortName;
            this.IRAAccountType = iRAAccountType;
        }

        public override PersonalIdentity Identity { get; }

        public IRAAccountType IRAAccountType { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Identity;
            yield return this.IRAAccountType;
        }
    }
}
