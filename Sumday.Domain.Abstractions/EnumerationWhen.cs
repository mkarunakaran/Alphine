using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions
{
    public readonly struct EnumerationWhen<TEnumeration, TValue>
       where TEnumeration : Enumeration<TEnumeration, TValue>
       where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly Enumeration<TEnumeration, TValue> enumeration;
        private readonly bool stopEvaluating;

        internal EnumerationWhen(bool stopEvaluating, Enumeration<TEnumeration, TValue> enumeration)
        {
            this.stopEvaluating = stopEvaluating;
            this.enumeration = enumeration;
        }

        public void Default(Action action)
        {
            if (!this.stopEvaluating)
            {
                action();
            }
        }

        public EnumerationThen<TEnumeration, TValue> When(Enumeration<TEnumeration, TValue> enumerationWhen) =>
            new EnumerationThen<TEnumeration, TValue>(isMatch: this.enumeration.Equals(enumerationWhen), stopEvaluating: this.stopEvaluating, enumeration: this.enumeration);

        public EnumerationThen<TEnumeration, TValue> When(params Enumeration<TEnumeration, TValue>[] enumerationWhens) =>
            new EnumerationThen<TEnumeration, TValue>(isMatch: enumerationWhens.Contains(this.enumeration), stopEvaluating: this.stopEvaluating, enumeration: this.enumeration);

        public EnumerationThen<TEnumeration, TValue> When(IEnumerable<Enumeration<TEnumeration, TValue>> enumerationWhens) =>
            new EnumerationThen<TEnumeration, TValue>(isMatch: enumerationWhens.Contains(this.enumeration), stopEvaluating: this.stopEvaluating, enumeration: this.enumeration);
    }
}
