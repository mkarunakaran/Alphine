using System;

namespace Sumday.Service.ShareHolder.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MaskedAttribute : Attribute
    {
    }
}
