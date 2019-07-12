using System;
using System.Linq.Expressions;

namespace DeReviewer.KnowledgeBase.Internals
{
    internal class PatternBuilder<T>
    {
        private readonly Context context;
        private readonly It it;
        private Version requiredOlderVersion = new Version();

        public PatternBuilder(Context context)
        {
            this.context = context;
            it = new It(context);
        }

        public PatternBuilder<T> AssemblyVersionOlderThan(int major, int minor, int build = 0, int revision = 0)
        {
            requiredOlderVersion = new Version(major, minor, build, revision);
            return this;
        }

        public void CreateBySignature(Expression<Action<It>> expression)
        {
            switch (context.Mode)
            {
                case ExecutionMode.Analyze:
                    Analyze(expression);
                    return;

                case ExecutionMode.Test:
                    var action = expression.Compile();
                    action(it);
                    return;

                default:
                    throw new NotSupportedException($"Unknown mode {context.Mode.ToString()}");
            }
        }

        public TResult CreateBySignature<TResult>(Expression<Func<It, TResult>> expression)
        {
            switch (context.Mode)
            {
                case ExecutionMode.Analyze:
                    Analyze(expression);
                    return default(TResult);

                case ExecutionMode.Test:
                    var func = expression.Compile();
                    return func(it);

                default:
                    throw new NotSupportedException($"Unknown mode {context.Mode.ToString()}");
            }
        }
        
        public void CreateByName(Expression<Action<It>> expression)
        {
            throw new NotImplementedException();
        }

        public TResult CreateByName<TResult>(Expression<Func<It, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        private void Analyze(LambdaExpression expression)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                context.Patterns.Add(new PatternInfo(
                    new MethodUniqueName(methodCall.Method),
                    requiredOlderVersion));
                        
                return;
            }

            throw new NotSupportedException($"The pattern '{expression}' doesn't contain a method call");
        }
    }
}