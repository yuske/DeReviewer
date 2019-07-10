namespace DeReviewer.KnowledgeBase
{
    public abstract class Case
    {
        public void Initialize(Context context)
        {
            Pattern = new Pattern(context);
            It = new It(context);
        }
        
        private protected Pattern Pattern { get; private set; }
        
        private protected It It { get; private set; }
    }
}