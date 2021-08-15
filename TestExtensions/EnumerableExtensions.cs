using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestExtensions.Internal;

namespace TestExtensions
{
    public static class EnumerableExtensions
    {
        public static MatchAnyResult MatchAny<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            var bestMatch = new MatchAny<T>(source, predicate);

            return bestMatch.GetResult();
        }
    }
}
