using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BestMatch
{
    public static class BestMatchExtensions
    {
        public static Result AnyBestMatch<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            bool isMatch = source.Any(s => predicate.Compile().Invoke(s));

            if (isMatch)
            {
                return Result.CreateEqual();
            }

            var body = predicate.Body;

            if (body is BinaryExpression binaryExpression)
            {
                var leftExpression = binaryExpression.Left;
                var expressionEvaluatesToTrue = EvaluateExpression(leftExpression);
            }

            return Result.CreateNotEqual(string.Empty);
        }

        private static bool EvaluateExpression(Expression expression)
        {
            if (expression.NodeType != ExpressionType.Equal)
            {
                throw new InvalidOperationException($"Expression {nameof(Expression.NodeType)} must be of type {ExpressionType.Equal}");
            }

            return Expression.Lambda<Func<bool>>(expression).Compile()();
        }
    }
}
