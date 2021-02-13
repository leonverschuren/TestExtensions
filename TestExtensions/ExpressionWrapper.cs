using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestExtensions
{
        internal class ExpressionWrapper<T>
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
