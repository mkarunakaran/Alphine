using System;

namespace Sumday.Infrastructure.Extensions
{
    public static class GuidExtensions
    {
        public static string ToBase64(this Guid value)
        {
            return Convert.ToBase64String(value.ToByteArray())
                .Replace("/", "-")
                .Replace("+", "_")
                .Replace("=", string.Empty);
        }
    }
}
