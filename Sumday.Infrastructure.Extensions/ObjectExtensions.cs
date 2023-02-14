using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Sumday.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetPropValue(this object src, string propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (propertyName.Contains("."))
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                return GetPropValue(GetPropValue(src, temp[0], bindingFlags), temp[1], bindingFlags);
            }
            else
            {
                var property = default(PropertyInfo);

                // add a check here that the object obj and propertyName string are not null
                foreach (var prop in src.GetType().GetProperties(bindingFlags))
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

                return property?.GetValue(src, null);
            }
        }

        public static PropertyInfo GetPropInfo(this object src, string propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (propertyName.Contains("."))
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                return GetPropInfo(GetPropValue(src, temp[0], bindingFlags), temp[1], bindingFlags);
            }
            else
            {
                var property = default(PropertyInfo);

                // add a check here that the object obj and propertyName string are not null
                foreach (var prop in src.GetType().GetProperties(bindingFlags))
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

        public static T GetPropValue<T>(this object obj, string name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null)
            {
                return default;
            }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static void SetPropValue(this object obj, string propertyName, object propertyValue, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            if (propertyName.Contains("."))
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                SetPropValue(GetPropValue(obj, temp[0], bindingFlags), temp[1], propertyValue, bindingFlags);
            }
            else
            {
                var propertyDetail = default(PropertyInfo);

                // add a check here that the object obj and propertyName string are not null
                foreach (var prop in obj.GetType().GetProperties(bindingFlags))
                {
                    if (prop.Name.ToLower().Contains(propertyName.ToLower()))
                    {
                        propertyDetail = prop;
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
                                propertyDetail = prop;
                                break;
                            }
                        }
                    }
                }

                if (propertyDetail != null && propertyDetail.CanWrite)
                {
                    Type propertyType = propertyDetail.PropertyType;

                    // Check for nullable types
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // Check for null or empty string value.
                        if (propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.ToString()))
                        {
                            propertyDetail.SetValue(obj, null);
                            return;
                        }
                        else
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                        }
                    }

                    try
                    {
                        var propertyNewValue = Convert.ChangeType(propertyValue, propertyType);
                        propertyDetail.SetValue(obj, propertyNewValue);
                        return;
                    }
                    catch (Exception)
                    {
                    }

                    propertyDetail.SetValue(obj, propertyValue);
                }
            }
        }

        public static void CopyObject(this object opSource, object opTarget, bool deepCopy = false)
        {
            // grab the type and create a new instance of that type
            Type opSourceType = opSource.GetType();

            // grab the properties
            PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // iterate over the properties and if it has a 'set' method assign it from the source TO the target
            foreach (var item in GetAllProperties(opSourceType.GetTypeInfo()))
            {
                var sourceData = item.GetValue(opSource, null);
                if (item.CanWrite && sourceData != null)
                {
                    var collectionInterface = typeof(IDictionary<,>).FindOpenGenericInterface(item.PropertyType);

                    if (collectionInterface != null)
                    {
                        var dictData = sourceData as IDictionary;
                        var typeInfo = dictData.GetType().GetTypeInfo();
                        var setter = typeInfo.GetDeclaredProperty("Item");
                        var targetDict = item.GetValue(opTarget, null);
                        if (targetDict == null)
                        {
                            targetDict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments[0], typeInfo.GenericTypeArguments[1]));
                            item.SetValue(opTarget, targetDict);
                        }

                        foreach (DictionaryEntry pair in dictData)
                        {
                            setter.SetValue(targetDict, pair.Value, new object[] { pair.Key });
                        }
                    }
                    else if ((collectionInterface = typeof(IEnumerable<>).FindOpenGenericInterface(item.PropertyType)) != null)
                    {
                        var enumerableData = sourceData as IEnumerable;
                        var typeInfo = enumerableData.GetType().GetTypeInfo();
                        var addMethod = typeInfo.GetDeclaredMethod("Add");
                        var targetEnumerable = item.GetValue(opTarget, null);
                        if (targetEnumerable == null)
                        {
                            targetEnumerable = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeInfo.GenericTypeArguments[0]));
                            item.SetValue(opTarget, targetEnumerable);
                        }

                        foreach (var data in enumerableData)
                        {
                            addMethod.Invoke(targetEnumerable, new[] { data });
                        }
                    }
                    else if (item.PropertyType.IsArray)
                    {
                        var elementType = item.PropertyType.GetElementType();
                        var soureArray = sourceData as Array;
                        var arrayLength = soureArray?.Length ?? 0;
                        if (arrayLength > 0)
                        {
                            if (item.GetValue(opTarget, null) is not Array targetArray)
                            {
                                targetArray = Array.CreateInstance(elementType, arrayLength);
                                item.SetValue(opTarget, targetArray);
                            }

                            Array.Copy(soureArray, targetArray, arrayLength);
                        }
                    }

                    // value types can simply be 'set'
                    else if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(string)))
                    {
                        item.SetValue(opTarget, sourceData, null);
                    }
                    else if (deepCopy)
                    {
                        var targetObj = item.GetValue(opTarget, null);
                        if (targetObj == null)
                        {
                            targetObj = Activator.CreateInstance(item.PropertyType);
                            item.SetValue(opTarget, targetObj);
                        }

                        sourceData.CopyObject(targetObj, deepCopy);
                    }
                }
            }
        }

        public static T Cast<T>(this object entity)
            where T : class
        {
            return entity as T;
        }

        public static dynamic DynamicCast(this object entity, Type to)
        {
            var openCast = typeof(ObjectExtensions).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public);
            var closeCast = openCast.MakeGenericMethod(to);
            return closeCast.Invoke(entity, new[] { entity });
        }

        public static string GetObjectKey(this object entity)
        {
           var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).OrderBy(prp => prp.Name).ToList();
           return string.Join(",", props.Select(prp => $"{prp.Name}-{prp.GetValue(entity)}"));
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type)
        {
            var allProperties = new List<PropertyInfo>();

            do
            {
                allProperties.AddRange(type.DeclaredProperties);
                type = type.BaseType.GetTypeInfo();
            }
            while (type != typeof(object).GetTypeInfo());

            return allProperties;
        }
    }
}