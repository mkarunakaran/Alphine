using System;

namespace Sumday.Domain.Abstractions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EntityKeyAttribute : Attribute
    {
    }
}
