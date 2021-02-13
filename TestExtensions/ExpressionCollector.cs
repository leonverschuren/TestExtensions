using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestExtensions
{
    public class ExpressionCollector
    {
        private readonly ICollection<BinaryExpression> _expressions;

        private ExpressionCollector()
        {
            _expressions = new List<BinaryExpression>();
        }

        private void Traverse(Expression expression)
        {
            if (expression is not BinaryExpression binaryExpression)
            {
                return;
            }

            if (expression.NodeType == ExpressionType.Equal)
            {
                _expressions.Add(binaryExpression);
            }
            else
            {
                Traverse(binaryExpression.Left);
                Traverse(binaryExpression.Right);
            }
        }

        public static IEnumerable<BinaryExpression> Collect(Expression expression)
        {
            var collector = new ExpressionCollector();
            collector.Traverse(expression);

            return collector._expressions;
        }
    }
}
