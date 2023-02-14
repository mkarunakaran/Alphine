using System;
using System.Reflection;

namespace Sumday.Infrastructure.Extensions
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberInfoType(this MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            if (memberInfo is FieldInfo fieldInfo)
            {
                return fieldInfo.FieldType;
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.PropertyType;
            }

            throw new NotSupportedException("Only field and properties are supported at this time.");
        }
    }
}
