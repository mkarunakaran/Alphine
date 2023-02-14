using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sumday.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common.Caching
{
    public class CachingDataContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!prop.Writable && member is PropertyInfo property)
            {
                var hasPrivateSetter = property?.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }

            if (!prop.Writable && member is FieldInfo)
            {
                prop.Writable = true;
            }

            prop.Readable = true;
            return prop;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            MemberInfo[] fields = objectType.GetFields(flags).Where(fld => fld.FieldType.Name == typeof(List<>).Name).Select(fld => fld as MemberInfo).ToArray();
            var props = fields
                .Concat(objectType.GetProperties(flags))
                .ToList();

            return props;
        }
    }
}
