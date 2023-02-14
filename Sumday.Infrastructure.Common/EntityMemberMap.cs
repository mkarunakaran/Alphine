using System;
using System.Linq.Expressions;
using System.Reflection;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common
{
    public class EntityMemberMap
    {
        private readonly BaseAggregateClassMap aggregateClassMap;
        private readonly MemberInfo memberInfo;
        private readonly Type memberType;
        private Func<object, object> getter;
        private Action<object, object> setter;

        public EntityMemberMap(BaseAggregateClassMap classMap, MemberInfo memberInfo)
        {
            this.aggregateClassMap = classMap;
            this.memberInfo = memberInfo;
            this.memberType = memberInfo.GetMemberInfoType();
        }

        public BaseAggregateClassMap AggregateClassMap
        {
            get { return this.aggregateClassMap; }
        }

        public string MemberName
        {
            get { return this.memberInfo.Name; }
        }

        public Type MemberType
        {
            get { return this.memberType; }
        }

        public MemberInfo MemberInfo
        {
            get { return this.memberInfo; }
        }

        public Func<object, object> Getter
        {
            get
            {
                if (this.getter == null)
                {
                    this.getter = this.GetGetter();
                }

                return this.getter;
            }
        }

        public Action<object, object> Setter
        {
            get
            {
                if (this.setter == null)
                {
                    this.setter = this.GetPropertySetter();
                }

                return this.setter;
            }
        }

        private Func<object, object> GetGetter()
        {
            var propertyInfo = this.memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                var getMethodInfo = propertyInfo.GetMethod;
                if (getMethodInfo == null)
                {
                    var message = string.Format(
                        "The property '{0} {1}' of class '{2}' has no 'get' accessor.",  propertyInfo.PropertyType.FullName, propertyInfo.Name, propertyInfo.DeclaringType.FullName);
                    throw new Exception(message);
                }
            }

            // lambdaExpression = (obj) => (object) ((TClass) obj).Member
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var lambdaExpression = Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.MakeMemberAccess(
                        Expression.Convert(objParameter, this.memberInfo.DeclaringType),
                        this.memberInfo),
                    typeof(object)),
                objParameter);

            return lambdaExpression.Compile();
        }

        private Action<object, object> GetPropertySetter()
        {
            var propertyInfo = (PropertyInfo)this.memberInfo;
            var setMethodInfo = propertyInfo.SetMethod;
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var valueParameter = Expression.Parameter(typeof(object), "value");
            var lambdaExpression = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                    Expression.Convert(objParameter, this.memberInfo.DeclaringType),
                    setMethodInfo,
                    Expression.Convert(valueParameter, this.memberType)),
                objParameter,
                valueParameter);

            return lambdaExpression.Compile();
        }
    }
}
