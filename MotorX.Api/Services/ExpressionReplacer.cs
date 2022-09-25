using System.Linq.Expressions;

namespace MotorX.Api.Services
{
    public class ExpressionReplacer: ExpressionVisitor
    {
        readonly IDictionary<Expression, Expression> _replaceMap;

        public ExpressionReplacer(IDictionary<Expression, Expression> replaceMap)
        {
            _replaceMap = replaceMap ?? throw new ArgumentNullException(nameof(replaceMap));
        }

        public override Expression Visit(Expression exp)
        {
            if (exp != null && _replaceMap.TryGetValue(exp, out var replacement))
                return replacement;
            return base.Visit(exp);
        }

        public static Expression Replace(Expression expr, Expression toReplace, Expression toExpr)
        {
            return new ExpressionReplacer(new Dictionary<Expression, Expression> { { toReplace, toExpr } }).Visit(expr);
        }

        public static Expression Replace(Expression expr, IDictionary<Expression, Expression> replaceMap)
        {
            return new ExpressionReplacer(replaceMap).Visit(expr);
        }

        public static Expression GetBody(LambdaExpression? lambda, params Expression[] toReplace)
        {
            if (lambda is not null)
            {
                if (lambda.Parameters.Count != toReplace.Length)
                    throw new InvalidOperationException();

                return new ExpressionReplacer(Enumerable.Zip(lambda.Parameters, toReplace, (f, s) => Tuple.Create(f, s))
                    .ToDictionary(e => (Expression)e.Item1, e => e.Item2)).Visit(lambda.Body);
            }
            return Expression.Empty();
        }
    }
}
