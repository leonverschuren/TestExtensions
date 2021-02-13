using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
            if (_predicate.Body is BinaryExpression binaryExpression)
            {
                foreach (T source in _source)
                {
                    var wrapper = new ExpressionWrapper(_predicate, binaryExpression);
                    bool evaluation = wrapper.Evaluate(source);

                    if (!evaluation)
                    {
                        return $"Expected '{wrapper.GetComparedMemberName()}' to be '{wrapper.GetExpectedValue()}', actual '{wrapper.GetActualValue(source)}'";
                    }
                }
            }

            return string.Empty;
        }

        private class ExpressionWrapper
        {
            private readonly Expression<Func<T, bool>> _predicate;
            private readonly BinaryExpression _expression;

            public ExpressionWrapper(Expression<Func<T, bool>> predicate, BinaryExpression expression)
            {
                _predicate = predicate;
                _expression = expression;
            }

            public bool Evaluate(T parameter)
            {
                Func<T, bool> compiledExpression = Expression.Lambda<Func<T, bool>>(_expression, _predicate.Parameters).Compile();

                return compiledExpression(parameter);
            }

            public string GetComparedMemberName()
            {
                if (_expression.Left is MemberExpression memberExpression)
                {
                    return memberExpression.Member.Name;
                }

                return string.Empty;
            }

            public string GetExpectedValue()
            {
                if (_expression.Right is ConstantExpression constantExpression)
                {
                    return constantExpression.Value.ToString();
                }

                return string.Empty;
            }

            public string GetActualValue(T value)
            {
                if (_expression.Left is MemberExpression memberExpression)
                {
                    return memberExpression.Member switch
                    {
                        PropertyInfo propertyInfo => propertyInfo.GetValue(value).ToString(),
                        FieldInfo fieldInfo       => fieldInfo.GetValue(value).ToString(),
                        _                         => string.Empty
                    };
                }

                return string.Empty;
            }
        }
    }
}
