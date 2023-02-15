using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public sealed class IRAAccountType : Enumeration<IRAAccountType, string>
    {
        private IRAAccountType(string type, string value)
         : base(type, value)
        {
        }

        public static IRAAccountType RothIRA => new IRAAccountType("422", nameof(RothIRA));

        public static IRAAccountType TraditionalIRA => new IRAAccountType("408", nameof(TraditionalIRA));

        public static implicit operator IRAAccountType(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(IRAAccountType accountType) => accountType.Value;
    }
}
