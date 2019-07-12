using System.IO;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer.KnowledgeBase
{
    internal class It
    {
        private readonly Context context;

        public It(Context context)
        {
            this.context = context;
        }
        
        public PayloadBuilder<TGadget> IsPayloadOf<TGadget>()
            where TGadget : IGadget, new()
        {
            return new PayloadBuilder<TGadget>(context);
        }
    
        public Payload IsPayloadFrom(string fileName)
        {
            var data = File.ReadAllText($@"Payloads\{fileName}");
            return new Payload(data.Replace("%CMD%", context.PayloadCommand));
        }
    }
}