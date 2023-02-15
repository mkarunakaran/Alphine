using Sumday.Domain.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public class IRAEmployerList : ValueObject, IEnumerable<IRAEmployer>
    {
        private readonly List<IRAEmployer> iRAEmployers;

        public IRAEmployerList(IEnumerable<IRAEmployer> iRAEmployers)
        {
            this.iRAEmployers = iRAEmployers.ToList();
        }

        public IEnumerator<IRAEmployer> GetEnumerator()
        {
            return this.iRAEmployers.GetEnumerator();
        }

        public override bool Equals(object other)
        {
            var otherIRAEmployerList = other as IRAEmployerList;
            return this.iRAEmployers
                .OrderBy(x => x.Id)
                .SequenceEqual(otherIRAEmployerList.iRAEmployers.OrderBy(x => x.Id));
        }

        public override int GetHashCode()
        {
            return this.iRAEmployers.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddIRAEmployer(IRAEmployer iRABeneficiary)
        {
            this.iRAEmployers.Add(iRABeneficiary);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var iRABeneficiary in this.iRAEmployers)
            {
                yield return iRABeneficiary;
            }
        }
    }
}
