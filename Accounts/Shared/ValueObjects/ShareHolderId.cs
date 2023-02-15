using System;
using System.Collections.Generic;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public sealed class ShareHolderId : ValueObject
    {
        public ShareHolderId(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static implicit operator ShareHolderId(string value) => new ShareHolderId(value);

        public static implicit operator string(ShareHolderId shareHolderId) => shareHolderId.Value;

        public static string Create()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
               .Replace("/", "-")
               .Replace("+", "_")
               .Replace("=", string.Empty)
               .ToUpper();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
