using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public class IRABeneficiaryList : ValueObject, IEnumerable<IRABeneficiary>
    {
        private readonly List<IRABeneficiary> iRABeneficiaries;

        public IRABeneficiaryList(IEnumerable<IRABeneficiary> iRABeneficiaryies)
        {
            this.iRABeneficiaries = iRABeneficiaryies.ToList();
        }

        public IEnumerator<IRABeneficiary> GetEnumerator()
        {
            return this.iRABeneficiaries.GetEnumerator();
        }

        public override bool Equals(object other)
        {
            var otherIRABeneficiaryList = other as IRABeneficiaryList;
            return this.iRABeneficiaries
                .OrderBy(x => x.Id)
                .SequenceEqual(otherIRABeneficiaryList.iRABeneficiaries.OrderBy(x => x.Id));
        }

        public override int GetHashCode()
        {
            return this.iRABeneficiaries.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddIRABeneficiary(IRABeneficiary iRABeneficiary)
        {
            if (!this.iRABeneficiaries.Any(ira => ira.IRABeneficiaryType == IRABeneficiaryType.Primary)
                 && iRABeneficiary.IRABeneficiaryType == IRABeneficiaryType.Contingent)
            {
                throw new InvalidObjectException(nameof(IRABeneficiaryList), "You should have atleast one Primary Beneficiary before adding a Contingent Beneficiary");
            }

            if (iRABeneficiary.IRABeneficiaryType == IRABeneficiaryType.Primary)
            {
                var totalPrimaryPerccentDesignated = this.iRABeneficiaries.Where(ira => ira.IRABeneficiaryType == IRABeneficiaryType.Primary)
                    .Sum(ira => ira.PercentDesignated)
                    + iRABeneficiary.PercentDesignated;
                if (totalPrimaryPerccentDesignated > 100)
                {
                    throw new InvalidObjectException(nameof(IRABeneficiaryList), "Total Percentage of All Primary Beneficiaries should not exceed 100%");
                }
            }

            if (iRABeneficiary.IRABeneficiaryType == IRABeneficiaryType.Contingent)
            {
                var totalContingentPerccentDesignated = this.iRABeneficiaries.Where(ira => ira.IRABeneficiaryType == IRABeneficiaryType.Contingent)
                    .Sum(ira => ira.PercentDesignated)
                    + iRABeneficiary.PercentDesignated;
                if (totalContingentPerccentDesignated > 100)
                {
                    throw new InvalidObjectException(nameof(IRABeneficiaryList), "Total Percentage of All Contingent Beneficiaries should not exceed 100%");
                }
            }

            this.iRABeneficiaries.Add(iRABeneficiary);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var iRABeneficiary in this.iRABeneficiaries)
            {
                yield return iRABeneficiary;
            }
        }
    }
}
