using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions
{
    public readonly struct EnumerationThen<TEnumeration, TValue>
         where TEnumeration : Enumeration<TEnumeration, TValue>
         where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly bool isMatch;
        private readonly Enumeration<TEnumeration, TValue> enumeration;
        private readonly bool stopEvaluating;

        internal EnumerationThen(bool isMatch, bool stopEvaluating, Enumeration<TEnumeration, TValue> enumeration)
        {
            this.isMatch = isMatch;
            this.enumeration = enumeration;
            this.stopEvaluating = stopEvaluating;
        }

        public EnumerationWhen<TEnumeration, TValue> Then(Action doThis)
        {
            if (!this.stopEvaluating && this.isMatch)
            {
                doThis();
            }

            return new EnumerationWhen<TEnumeration, TValue>(this.stopEvaluating || this.isMatch, this.enumeration);
        }
    }
}
