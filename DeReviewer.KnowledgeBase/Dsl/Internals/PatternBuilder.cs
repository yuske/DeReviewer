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
        
        public void Create(Expression<Action<It>> expression)
        {
            AnalyzeOrTest(expression, () =>
            {
                var func = expression.Compile();
                func(it);
            });
        }

        public TResult Create<TResult>(Expression<Func<It, TResult>> expression)
        {
            var result = default(TResult);
            AnalyzeOrTest(expression, () =>
            {
                var func = expression.Compile();
                result = func(it);
            });

            return result;
        }

        private void AnalyzeOrTest(LambdaExpression expression, Action test)
        {
            switch (context.Mode)
            {
                case Context.ExecutionMode.Analyze:
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

                case Context.ExecutionMode.Test:
                {
                    test();
                    break;
                }
                
                default:
                    throw new NotSupportedException($"Unknown mode {context.Mode.ToString()}");
            }
        }
    }
}