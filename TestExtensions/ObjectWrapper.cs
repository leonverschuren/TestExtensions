using System;
using System.Collections.Generic;
using System.Linq;

namespace TestExtensions
{
    internal class ObjectWrapper<T>
    {
        private readonly T _value;
        private readonly Lazy<IDictionary<ExpressionWrapper<T>, bool>> _results;

        private IDictionary<ExpressionWrapper<T>, bool> Results => _results.Value;

        public ObjectWrapper(T value, IEnumerable<ExpressionWrapper<T>> expressionWrappers)
        {
            _value = value;
            _results = new Lazy<IDictionary<ExpressionWrapper<T>, bool>>(() => Evaluate(expressionWrappers, value));
        }

        public int MatchCount => Results.Count(r => r.Value);

        private static IDictionary<ExpressionWrapper<T>, bool> Evaluate(IEnumerable<ExpressionWrapper<T>> expressionWrappers, T value)
        {
            return expressionWrappers.ToDictionary(e => e, e => e.Evaluate(value));
        }

        public string GetMessage()
        {
            ExpressionWrapper<T> wrapper = Results.FirstOrDefault(r => !r.Value).Key;

            return $"Expected '{wrapper.GetComparedMemberName()}' to be '{wrapper.GetExpectedValue()}', actual '{wrapper.GetActualValue(_value)}'";;
        }
    }
}
