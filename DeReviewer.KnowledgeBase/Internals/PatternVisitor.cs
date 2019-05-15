using System.Linq.Expressions;

namespace DeReviewer.KnowledgeBase.Internals
{
    internal class PatternVisitor : ExpressionVisitor
    {
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            return base.VisitInvocation(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }
    }
}