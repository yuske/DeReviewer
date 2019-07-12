namespace DeReviewer.KnowledgeBase
{
    public abstract class Case
    {
        public void Initialize(Context context)
        {
            Pattern = new Pattern(context);
        }
        
        private protected Pattern Pattern { get; private set; }
    }
}