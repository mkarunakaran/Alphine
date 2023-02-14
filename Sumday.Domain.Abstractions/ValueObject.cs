using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sumday.Domain.Abstractions
{
    [Serializable]
    public abstract class ValueObject : IComparable, IComparable<ValueObject>
    {
        private int? cachedHashCode;

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            var valueObject = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            if (!this.cachedHashCode.HasValue)
            {
                this.cachedHashCode = this.GetEqualityComponents()
                    .Aggregate(1, (current, obj) =>
                    {
                        unchecked
                        {
                            return (current * 23) + (obj?.GetHashCode() ?? 0);
                        }
                    });
            }

            return this.cachedHashCode.Value;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            object[] components = this.GetEqualityComponents().ToArray();

            for (int i = 0; i < components.Length; i++)
            {
                string componentvalue = components[i] as string;
                if (!string.IsNullOrEmpty(componentvalue))
                {
                    stringBuilder.Append(componentvalue);
                }
            }

            return stringBuilder.ToString();
        }

        public virtual int CompareTo(object obj)
        {
            Type thisType = this.GetType();
            Type otherType = obj.GetType();

            if (thisType != otherType)
            {
                return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);
            }

            var other = (ValueObject)obj;

            object[] components = this.GetEqualityComponents().ToArray();
            object[] otherComponents = other.GetEqualityComponents().ToArray();

            for (int i = 0; i < components.Length; i++)
            {
                int comparison = CompareComponents(components[i], otherComponents[i]);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return 0;
        }

        public virtual int CompareTo(ValueObject other)
        {
            return this.CompareTo(other as object);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        private static int CompareComponents(object object1, object object2)
        {
            if (object1 is null && object2 is null)
            {
                return 0;
            }

            if (object1 is null)
            {
                return -1;
            }

            if (object2 is null)
            {
                return 1;
            }

            if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            {
                return comparable1.CompareTo(comparable2);
            }

            return object1.Equals(object2) ? 0 : -1;
        }
    }
}
