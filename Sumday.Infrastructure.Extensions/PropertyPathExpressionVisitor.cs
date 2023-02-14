using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Sumday.Infrastructure.Extensions
{
    public class PropertyPathExpressionVisitor : ExpressionVisitor
    {
        public PropertyPathExpressionVisitor()
        {
            this.Path = new Dictionary<string, Type>();
        }

        public IDictionary<string, Type> Path { get; }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Member is PropertyInfo))
            {
                throw new ArgumentException("The path can only contain properties.", nameof(node));
            }

            var memberInfo = node.Member;
            var memberType = node.Member.GetMemberInfoType();
            var name = memberInfo.DeclaringType.Name + "." + memberInfo.Name;

            if (!this.Path.ContainsKey(name))
            {
                this.Path.Add(name, memberType);
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitUnary(UnaryExpression unary)
        {
            if (unary.NodeType == ExpressionType.TypeAs)
            {
                if (unary.Operand is MemberExpression memberExpression)
                {
                    var memberInfo = memberExpression.Member;
                    var name = memberInfo.DeclaringType.Name + "." + memberInfo.Name;

                    if (!this.Path.ContainsKey(name))
                    {
                        this.Path.Add(name, unary.Type);
                    }
                }
            }

            return base.VisitUnary(unary);
        }

        private static string TypeName(Type type)
        {
            if (type.DeclaringType == null)
            {
                return type.Name;
            }

            return TypeName(type.DeclaringType) + "." + type.Name;
        }

        private static string GetTypeName(Type type)
        {
            if (type.MemberType == MemberTypes.NestedType)
            {
                return string.Concat(GetTypeName(type.DeclaringType), ".", type.Name);
            }

            return type.Name;
        }
    }
}
