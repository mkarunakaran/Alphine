using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common
{
    public static class ExpressionExtensions
    {
        public static IDictionary<string, Type> PropertyPath(this Expression selector)
        {
            var visitor = new PropertyPathExpressionVisitor();
            visitor.Visit(selector);
            return visitor.Path;
        }
    }
}
