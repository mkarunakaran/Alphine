using Sumday.BoundedContext.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public sealed class AccountNumber : Masked
    {
        private const int AccountNumberLength = 10;

        public AccountNumber(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static implicit operator AccountNumber(string value) => new AccountNumber(value);

        public static implicit operator string(AccountNumber accountNumber) => accountNumber.Value;

        public static string Create(string accountIndex, string prefix)
        {
            accountIndex = accountIndex.PadLeft(AccountNumberLength - prefix.Length - 1, '0');
            accountIndex = string.Concat(prefix, accountIndex);

            var checksum = GetChecksum(accountIndex);
            accountIndex = string.Concat(accountIndex, checksum);

            return accountIndex;
        }

        public static string Get7Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return string.Format("{0:D7}", random);
        }

        public override string ToMaskedString()
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                return this.Value;
            }

            return new string(Mask, this.Value.Length - 4)
                  + this.Value.Substring(this.Value.Length - 4);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        private static string GetChecksum(string accountNumber)
        {
            int i;
            var result = 0;

            for (i = 0; i < accountNumber.Length; i++)
            {
                var testChar = accountNumber[i];
                int charval;
                if (testChar >= '0' & testChar <= '9')
                {
                    charval = testChar - '0';
                }
                else if (testChar >= 'A' & testChar <= 'Z')
                {
                    charval = testChar - 'A' + 10;
                }
                else
                {
                    charval = 0;
                }

                if (i % 2 == 0)
                {
                    charval *= 2;
                }

                result = result + (charval / 100) + ((charval / 10) % 10) + (charval % 10);
            }

            result = 10 - (result % 10);
            if (result == 10)
            {
                result = 0;
            }

            return result.ToString(CultureInfo.InvariantCulture);
        }
    }
}
