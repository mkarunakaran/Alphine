using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects
{
    public class CollegeAccountOwner : AccountOwner
    {
        public CollegeAccountOwner(PersonalIdentity identity, Name shortName, CollegeAccountType collegeAccountType)
        {
            this.Identity = identity;
            this.ShortName = shortName;
            this.CollegeAccountType = collegeAccountType;
        }

        public override PersonalIdentity Identity { get; }

        public CollegeAccountType CollegeAccountType { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Identity;
            yield return this.CollegeAccountType;
        }
    }
}
