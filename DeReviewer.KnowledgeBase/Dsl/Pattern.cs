using System;
using System.Linq.Expressions;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer.KnowledgeBase
{
    internal class Pattern
    {
        private readonly Context context;

        public Pattern(Context context)
        {
            this.context = context;
        }
        
        public PatternBuilder<T> Of<T>() => 
            new PatternBuilder<T>(context);
        
        public void Create(Expression<Action> expression) => 
            new PatternBuilder<object>(context).Create(expression);

        public TResult Create<TResult>(Expression<Func<TResult>> expression) => 
            new PatternBuilder<object>(context).Create(expression);
    }
}