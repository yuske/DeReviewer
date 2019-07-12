namespace DeReviewer.KnowledgeBase.Internals
{
    internal class PayloadBuilder<TGadget>
        where TGadget : IGadget, new()
    {
        private readonly Context context;

        public PayloadBuilder(Context context)
        {
            this.context = context;
        }

        public Payload Format<TFormatter>()
            where TFormatter : IFormatter, new()
        {
            //TODO: add completed payload generation event
            var gadget = new TGadget();
            var formatter = new TFormatter();
            return formatter.GeneratePayload(gadget.Build(context.PayloadCommand));
        }
    }
}