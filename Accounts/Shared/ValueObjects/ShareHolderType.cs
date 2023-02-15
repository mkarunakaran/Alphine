using Sumday.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    /// <summary>
    /// A shareholder is a person who logs into the system.
    /// </summary>
    public sealed class ShareHolderType : Enumeration<ShareHolderType, string>
    {
        private const string Agent = "person who administers assets within the entity and manages the account on behalf of the entity for AbleEntity and CSPEntity account types";

        private const string Entity = "Entity";

        private const string Individual = "person who owns money/securities within the account and managess the Individual, ugma and utma account types";

        private const string Trustee = "person who has control or powers of administration of assets in a trust and manages Trust account type";

        private const string Trust = "Trust";

        private const string Employee = "person who meet the eligibility requirements for enrollment on their Employer's portal";

        private const string Employer = "Employer";

        public ShareHolderType(string type, string value)
         : base(type, value)
        {
        }

        public static ShareHolderType IndividualCustomer => new ShareHolderType(nameof(Individual), nameof(Individual));

        public static ShareHolderType AgentCustomer => new ShareHolderType(nameof(Entity), nameof(Agent));

        public static ShareHolderType TrusteeCustomer => new ShareHolderType(nameof(Trust), nameof(Trustee));

        public static ShareHolderType EmployeeCustomer => new ShareHolderType(nameof(Employee), nameof(Employer));

        public static implicit operator ShareHolderType(string type)
        {
            var types = GetConstantsValues<string>(typeof(ShareHolderType));
            var typevalue = types.FirstOrDefault(ty => ty == type);
            if (typevalue != null)
            {
                return FromValue(typevalue) ?? FromName(typevalue);
            }

            return null;
        }

        public static implicit operator string(ShareHolderType shareHolderType) => shareHolderType.Value;

        private static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }

        private static IEnumerable<T> GetConstantsValues<T>(Type type)
            where T : class
        {
            var fieldInfos = GetConstants(type);

            return fieldInfos.Select(fi => fi.GetRawConstantValue() as T);
        }
    }
}
