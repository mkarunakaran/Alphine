using System;
using System.Collections.Generic;
using System.Linq;

namespace Sumday.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Except(second, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Intersect(second, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Union(second, new LambdaComparer<TSource>(comparer));
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> first, TSource second, Func<TSource, TSource, bool> comparer)
        {
            return first.Contains(second, new LambdaComparer<TSource>(comparer));
        }

        public static IOrderedEnumerable<T> NullableOrderBy<T>(this IEnumerable<T> list, Func<T, string> keySelector)
        {
            return list.OrderBy(v => keySelector(v) != null ? 0 : 1).ThenBy(keySelector);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> lambdaComparer;
            private readonly Func<T, int> lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer)
                : this(lambdaComparer, o => 0)
            {
            }

            public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
            {
                this.lambdaComparer = lambdaComparer ?? throw new ArgumentNullException(nameof(lambdaComparer));
                this.lambdaHash = lambdaHash ?? throw new ArgumentNullException(nameof(lambdaHash));
            }

            public bool Equals(T x, T y)
            {
                return this.lambdaComparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return this.lambdaHash(obj);
            }
        }
    }
}
