using System;
using System.Linq.Expressions;

namespace DeReviewer.KnowledgeBase.Internals
{
    internal class PatternBuilder<T>
    {
        private readonly Mode mode;

        private Version requiredOlderVersion = new Version();

        public PatternBuilder(Mode mode)
        {
            this.mode = mode;
        }

        public PatternBuilder<T> AssemblyVersionOlderThan(int major, int minor, int build = 0, int revision = 0)
        {
            requiredOlderVersion = new Version(major, minor, build, revision);
            return this;
        }
        
        public void Create(Expression<Action> expression)
        {
            AnalyzeOrTest(expression, () =>
            {
                var func = expression.Compile();
                func();
            });
        }

        public TResult Create<TResult>(Expression<Func<TResult>> expression)
        {
            var result = default(TResult);
            AnalyzeOrTest(expression, () =>
            {
                var func = expression.Compile();
                result = func();
            });

            return result;
        }

        private void AnalyzeOrTest(LambdaExpression expression, Action test)
        {
            switch (mode)
            {
                case Mode.Analyze:
                {
                    if (expression.Body is MethodCallExpression methodCall)
                    {
                        Dsl.Patterns.Add(new PatternInfo(
                            new MethodUniqueName(methodCall.Method),
                            requiredOlderVersion));
                        
                        return;
                    }

                    throw new NotSupportedException($"The pattern '{expression}' doesn't contain a method call");
                }

                case Mode.Test:
                {
                    test();
                    break;
                }
                
                default:
                    throw new NotSupportedException($"Unknown mode {mode.ToString()}");
            }
        }
    }
}