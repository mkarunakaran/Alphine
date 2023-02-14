using System;
using System.Reflection;

namespace Sumday.Domain.Abstractions
{
    public abstract class Entity
    {
        protected Entity()
        {
            var type = this.GetType();
            if (!type.HasEntityKey())
            {
                throw new InvalidOperationException($"Entity {type.Name} must define a property marked with {nameof(EntityKeyAttribute)}");
            }
        }

        public static bool operator ==(Entity a, Entity b)
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

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            var thisEntityKey = this.GetEntityKey();
            var otherEntityKey = other.GetEntityKey();

            if (thisEntityKey.Equals(this.GetDefault(thisEntityKey.GetType())) || otherEntityKey.Equals(this.GetDefault(otherEntityKey.GetType())))
            {
                return false;
            }

            return thisEntityKey.Equals(otherEntityKey);
        }

        public override int GetHashCode()
        {
            var thisEntityKey = this.GetEntityKey();
            return (this.GetType().ToString() + thisEntityKey?.ToString()).GetHashCode();
        }

        public T GetDefaultGeneric<T>()
        {
            return default;
        }

        private object GetDefault(Type t)
        {
            var defaultValue = this.GetType()
                .GetRuntimeMethod(nameof(this.GetDefaultGeneric), Array.Empty<Type>())
                .MakeGenericMethod(t).Invoke(this, null);
            return defaultValue;
        }
    }
}
