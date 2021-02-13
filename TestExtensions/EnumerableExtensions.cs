using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestExtensions
{
    public static class EnumerableExtensions
    {
        public static Result AnyBestMatch<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            var bestMatch = new BestMatch<T>(source, predicate);

            return bestMatch.GetResult();
        }
    }
}
