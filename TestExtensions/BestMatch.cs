using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestExtensions
{
    internal class BestMatch<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Expression<Func<T, bool>> _predicate;

        public BestMatch(IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            _source = source;
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

        private bool IsMatch() => _source.Any(s => _predicate.Compile().Invoke(s));

        private string GetMessage()
        {
            var expressions = ExpressionCollector.Collect(_predicate.Body).ToArray();
            var wrappers = expressions.Select(e => new ExpressionWrapper<T>(_predicate, e)).ToArray();

            foreach (T source in _source)
            {
                foreach (var wrapper in wrappers)
                {
                    bool evaluation = wrapper.Evaluate(source);

                    if (!evaluation)
                    {
                        return $"Expected '{wrapper.GetComparedMemberName()}' to be '{wrapper.GetExpectedValue()}', actual '{wrapper.GetActualValue(source)}'";
                    }
                }
            }

            return string.Empty;
        }
    }
}
