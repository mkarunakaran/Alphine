using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common
{
    public class AggregateClassMap<TAggregate> : BaseAggregateClassMap
        where TAggregate : AggregateRoot
    {
        public AggregateClassMap()
           : base(typeof(TAggregate))
        {
        }

        public AggregateClassMap(Action<AggregateClassMap<TAggregate>> classMapInitializer)
               : base(typeof(TAggregate))
        {
            classMapInitializer(this);
        }

        public EntityMemberMap GetMemberMap<TMember>(Expression<Func<TAggregate, TMember>> memberLambda)
            where TMember : Entity
        {
            var memberName = GetMemberNameFromLambda(memberLambda);
            return this.GetMemberMap(memberName);
        }

        public EntityMemberMap GetMemberMap(string memberName)
        {
            if (memberName == null)
            {
                throw new ArgumentNullException(nameof(memberName));
            }

            return this.DeclaredMemberMaps.SingleOrDefault(m => m.MemberName == memberName);
        }

        private static MemberInfo GetMemberInfoFromLambda<TMember>(Expression<Func<TAggregate, TMember>> memberLambda)
        {
            var body = memberLambda.Body;
            MemberExpression memberExpression;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    memberExpression = (MemberExpression)body;
                    break;
                case ExpressionType.Convert:
                    var convertExpression = (UnaryExpression)body;
                    memberExpression = (MemberExpression)convertExpression.Operand;
                    break;
                default:
                    throw new Exception("Invalid lambda expression");
            }

            var memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo info)
            {
                if (memberInfo.DeclaringType.GetTypeInfo().IsInterface)
                {
                    memberInfo = FindPropertyImplementation(info, typeof(TAggregate));
                }
            }
            else if (!(memberInfo is FieldInfo))
            {
                throw new Exception("Invalid lambda expression");
            }

            return memberInfo;
        }

        private static string GetMemberNameFromLambda<TMember>(Expression<Func<TAggregate, TMember>> memberLambda)
        {
            return GetMemberInfoFromLambda(memberLambda).Name;
        }

        private static PropertyInfo FindPropertyImplementation(PropertyInfo interfacePropertyInfo, Type actualType)
        {
            var interfaceType = interfacePropertyInfo.DeclaringType;

            // An interface map must be used because because there is no
            // other officially documented way to derive the explicitly
            // implemented property name.
            var interfaceMap = actualType.GetInterfaceMap(interfaceType);

            var interfacePropertyAccessors = GetPropertyAccessors(interfacePropertyInfo);

            var actualPropertyAccessors = interfacePropertyAccessors.Select(interfacePropertyAccessor =>
            {
                var index = Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, interfacePropertyAccessor);

                return interfaceMap.TargetMethods[index];
            });

            // Binding must be done by accessor methods because interface
            // maps only map accessor methods and do not map properties.
            return actualType.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single(propertyInfo =>
                {
                    // we are looking for a property that implements all the required accessors
                    var propertyAccessors = GetPropertyAccessors(propertyInfo);
                    return actualPropertyAccessors.All(x => propertyAccessors.Contains(x));
                });
        }

        private static MethodInfo[] GetPropertyAccessors(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetAccessors(true);
        }
    }
}
