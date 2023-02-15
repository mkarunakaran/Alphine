using Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice
{
    public class SecureChoiceBeneficiary : Entity
    {
        public SecureChoiceBeneficiary(string id)
        {
            this.Id = id;
            this.IRABeneficiaryList = new IRABeneficiaryList(new List<IRABeneficiary>());
            this.IRAEmployerList = new IRAEmployerList(new List<IRAEmployer>());
        }

        [EntityKey]
        public string Id { get; }

        public IRABeneficiaryList IRABeneficiaryList { get; }

        public IRAEmployerList IRAEmployerList { get; }
    }
}
