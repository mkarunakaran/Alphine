using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public abstract class AccountOwner : ValueObject
    {
          public abstract PersonalIdentity Identity { get; }

          public Name ShortName { get; protected set; }
    }
}
