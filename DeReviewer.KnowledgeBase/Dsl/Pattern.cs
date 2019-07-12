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
        
        public void CreateBySignature(Expression<Action<It>> expression) => 
            new PatternBuilder<object>(context).CreateBySignature(expression);

        public TResult CreateBySignature<TResult>(Expression<Func<It, TResult>> expression) => 
            new PatternBuilder<object>(context).CreateBySignature(expression);
        
        public void CreateByName(Expression<Action<It>> expression) => 
            new PatternBuilder<object>(context).CreateByName(expression);

        public TResult CreateByName<TResult>(Expression<Func<It, TResult>> expression) => 
            new PatternBuilder<object>(context).CreateByName(expression);
    }
}