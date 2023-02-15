using System;
using System.Collections.Generic;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public sealed class AccountId : ValueObject
    {
        public AccountId(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static implicit operator AccountId(string value) => new AccountId(value);

        public static implicit operator string(AccountId accountId) => accountId.Value;

        public static string Create()
        {
            return string.Empty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
