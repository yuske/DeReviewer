using System;
using System.Linq.Expressions;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer.KnowledgeBase
{
    internal static class Pattern
    {
        public static PatternBuilder<T> Of<T>() => 
            new PatternBuilder<T>(Dsl.Mode);
        
        public static void Create(Expression<Action> expression) => 
            new PatternBuilder<object>(Dsl.Mode).Create(expression);

        public static TResult Create<TResult>(Expression<Func<TResult>> expression) => 
            new PatternBuilder<object>(Dsl.Mode).Create(expression);
    }
}