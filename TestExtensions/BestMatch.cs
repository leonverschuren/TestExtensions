using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestExtensions
{
    internal class BestMatch<T>
    {
        private readonly IEnumerable<T> _sources;
        private readonly Expression<Func<T, bool>> _predicate;

        public BestMatch(IEnumerable<T> sources, Expression<Func<T, bool>> predicate)
        {
            _sources = sources;
            _predicate = predicate;
        }

        public Result GetResult()
        {
            bool isMatch = IsMatch();
            if (isMatch)
            {
                return Result.CreateEqual();
            }

            string message = GetMessage();

            return Result.CreateNotEqual(message);
        }

        private bool IsMatch() => _sources.Any(s => _predicate.Compile().Invoke(s));

        private string GetMessage()
        {
            ExpressionWrapper<T>[] wrappers = ExpressionCollector
                .Collect(_predicate.Body)
                .Select(e => new ExpressionWrapper<T>(_predicate.Parameters, e))
                .ToArray();

            IEnumerable<ObjectWrapper<T>> objectWrappers = _sources.Select(s => new ObjectWrapper<T>(s, wrappers));
            ObjectWrapper<T> bestMatch = objectWrappers.OrderByDescending(o => o.MatchCount).First();

            return bestMatch.GetMessage();
        }
    }
}
