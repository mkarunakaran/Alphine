using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Sumday.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        public static object GetDefault(this Type t)
        {
            var defaultValue = typeof(TypeExtensions)
                .GetRuntimeMethod(nameof(GetDefaultGeneric), Array.Empty<Type>())
                .MakeGenericMethod(t).Invoke(null, null);
            return defaultValue;
        }

        public static T GetDefaultGeneric<T>()
        {
            return default;
        }

        public static PropertyInfo GetPropInfo(this Type objType, string propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (propertyName.Contains("."))
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                return GetPropInfo(GetPropInfo(objType, temp[0], bindingFlags)?.PropertyType, temp[1], bindingFlags);
            }
            else
            {
                var property = default(PropertyInfo);

                // add a check here that the object obj and propertyName string are not null
                foreach (var prop in objType.GetProperties(bindingFlags))
                {
                    if (prop.Name.ToLower().Contains(propertyName.ToLower()))
                    {
                        property = prop;
                        break;
                    }
                    else
                    {
                        if (prop.PropertyType.IsAbstract)
                        {
                            var types = prop.PropertyType.Assembly.ExportedTypes.Where(t => t.IsSubclassOf(prop.PropertyType) && !t.IsAbstract).ToList();
                            var type = types.FirstOrDefault(ty => ty.Name == propertyName);
                            if (type != null)
                            {
                                property = prop;
                                break;
                            }
                        }
                    }
                }

                return property;
            }
        }

        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive ||
                   type.IsValueType ||
                   type == typeof(string);
        }

        public static string GetRootNodeElementNameSpaceForType(this Type serializedObjectType)
        {
            var rootAttribute = serializedObjectType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>();

            if (rootAttribute != null)
            {
                if (!string.IsNullOrEmpty(rootAttribute.Namespace))
                {
                    return rootAttribute.Namespace;
                }
                else
                {
                    return serializedObjectType.Namespace;
                }
            }
            else
            {
                return serializedObjectType.Namespace;
            }
        }

        public static string GetRootNodeElementNameForType(this Type serializedObjectType)
        {
            var rootAttribute = serializedObjectType.GetTypeInfo().GetCustomAttribute<XmlRootAttribute>();

            if (rootAttribute != null)
            {
                if (!string.IsNullOrEmpty(rootAttribute.ElementName))
                {
                    return rootAttribute.ElementName;
                }
                else
                {
                    return serializedObjectType.Name;
                }
            }
            else
            {
                return serializedObjectType.Name;
            }
        }

        public static bool Implements(this Type @this, Type @interface)
        {
            if (@this == null || @interface == null)
            {
                return false;
            }

            return @interface.GenericTypeArguments.Length > 0
                ? @interface.IsAssignableFrom(@this)
                : @this.GetInterfaces().Any(c => c.Name == @interface.Name);
        }

        public static Type FindOpenGenericInterface(this Type actual, Type expected)
        {
            var actualTypeInfo = actual.GetTypeInfo();
            if (actualTypeInfo.IsGenericType &&
                actual.GetGenericTypeDefinition().Name == expected.Name)
            {
                return actual;
            }

            var interfaces = actualTypeInfo.ImplementedInterfaces;
            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.GetTypeInfo().IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == expected)
                {
                    return interfaceType;
                }
            }

            return null;
        }
    }
}
