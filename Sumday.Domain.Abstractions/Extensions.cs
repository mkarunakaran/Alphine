using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sumday.Domain.Abstractions
{
    public static class Extensions
    {
        public static object GetEntityKey(this Entity entity)
        {
            return entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => Attribute.IsDefined(p, typeof(EntityKeyAttribute)))
                         .Select(prop => prop.GetValue(entity, null))
                         .Where(propVal => propVal != null)
                         .SingleOrDefault();
        }

        public static string GetEntityKeyWithName(this Entity entity)
        {
            return entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => Attribute.IsDefined(p, typeof(EntityKeyAttribute)))
                         .Select(prop => $"{prop.Name}-{prop.GetValue(entity)}")
                          .SingleOrDefault();
        }

        public static bool HasEntityKey(this Type entityType)
        {
            return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .SingleOrDefault(p => Attribute.IsDefined(p, typeof(EntityKeyAttribute))) != null;
        }

        public static IEnumerable<FieldInfo> GetConstants(this Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }

        public static IEnumerable<T> GetConstantsValues<T>(this Type type)
            where T : class
        {
            var fieldInfos = GetConstants(type);

            return fieldInfos.Select(fi => fi.GetRawConstantValue() as T);
        }
    }
}
