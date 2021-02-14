using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace TestExtensions
{
    internal class ExpressionWrapper<T>
    {
        private readonly IEnumerable<ParameterExpression> _parameters;
        private readonly BinaryExpression _expression;

        public ExpressionWrapper(IEnumerable<ParameterExpression> parameters, BinaryExpression expression)
        {
            _parameters = parameters;
            _expression = expression;
        }

        public bool Evaluate(T parameter)
        {
            Func<T, bool> compiledExpression = Expression.Lambda<Func<T, bool>>(_expression, _parameters).Compile();

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
            return _expression.Right switch
            {
                ConstantExpression constantExpression => constantExpression.Value.ToString(),
                MemberExpression memberExpression     => GetMemberExpressionValue(memberExpression, GetRootObject(memberExpression)),
                _                                     => string.Empty
            };
        }

        public string GetActualValue(T value)
        {
            if (_expression.Left is MemberExpression memberExpression)
            {
                return GetMemberExpressionValue(memberExpression, value);
            }

            return string.Empty;
        }

        private static string GetMemberExpressionValue(MemberExpression memberExpression, object value)
        {
            return memberExpression.Member switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(value).ToString(),
                FieldInfo fieldInfo       => fieldInfo.GetValue(value).ToString(),
                _                         => string.Empty
            };
        }

        private static object GetRootObject(MemberExpression expression)
        {
            MemberExpression memberExpression = expression;

            while (memberExpression.Expression is MemberExpression innerExpression)
            {
                memberExpression = innerExpression;
            }

            if (memberExpression.Expression is ConstantExpression rootObjectConstantExpression && memberExpression.Member is FieldInfo fieldInfo)
            {
                return fieldInfo.GetValue(rootObjectConstantExpression.Value);
            }

            throw new InvalidOperationException("Unable to get root object from expression.");
        }
    }
}
