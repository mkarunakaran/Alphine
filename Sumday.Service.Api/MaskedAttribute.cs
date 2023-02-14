using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MaskedAttribute : Attribute
    {
    }
}
