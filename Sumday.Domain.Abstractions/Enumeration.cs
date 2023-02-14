using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sumday.Domain.Abstractions
{
    public abstract class Enumeration<TEnumeration, TValue> : IEquatable<Enumeration<TEnumeration, TValue>>, IComparable<Enumeration<TEnumeration, TValue>>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private static readonly Dictionary<TValue, TEnumeration> EnumerationsByValue = GetEnumerations().ToDictionary(e => e.Value);
        private static readonly Dictionary<string, TEnumeration> EnumerationsByName = GetEnumerations().ToDictionary(e => e.Name);

        protected Enumeration(TValue value, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name cannot be null or empty");
            }

            this.Value = value;
            this.Name = name;
        }

        public TValue Value { get; protected set; }

        public string Name { get; protected set; }

        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator <(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) =>
           left.CompareTo(right) < 0;

        public static bool operator <=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) =>
            left.CompareTo(right) > 0;

        public static bool operator >=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) =>
            left.CompareTo(right) >= 0;

        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right) =>
            !(left == right);

        public static TEnumeration FromValue(TValue value)
        {
            return EnumerationsByValue.ContainsKey(value)
                ? EnumerationsByValue[value]
                : null;
        }

        public static TEnumeration FromName(string name)
        {
            return EnumerationsByName.ContainsKey(name)
                ? EnumerationsByName[name]
                : null;
        }

        public virtual int CompareTo(Enumeration<TEnumeration, TValue> other) => this.Value.CompareTo(other.Value);

        public EnumerationThen<TEnumeration, TValue> When(Enumeration<TEnumeration, TValue> enumerationWhen) =>
          new EnumerationThen<TEnumeration, TValue>(this.Equals(enumerationWhen), false, this);

        public EnumerationThen<TEnumeration, TValue> When(params Enumeration<TEnumeration, TValue>[] enumerations) =>
            new EnumerationThen<TEnumeration, TValue>(enumerations.Contains(this), false, this);

        public EnumerationThen<TEnumeration, TValue> When(IEnumerable<Enumeration<TEnumeration, TValue>> enumerations) =>
            new EnumerationThen<TEnumeration, TValue>(enumerations.Contains(this), false, this);

        public override bool Equals(object obj) => (obj is Enumeration<TEnumeration, TValue> other) && this.Equals(other);

        public virtual bool Equals(Enumeration<TEnumeration, TValue> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Value.Equals(other.Value);
        }

        public override int GetHashCode() => this.Value.GetHashCode();

        public override string ToString() => this.Name;

        private static TEnumeration[] GetEnumerations()
        {
            var enumerationType = typeof(TEnumeration);

            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => info.FieldType == typeof(TEnumeration))
                .Select(info => (TEnumeration)info.GetValue(null))
                .ToArray();
        }
    }
}
