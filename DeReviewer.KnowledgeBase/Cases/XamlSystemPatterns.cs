using System.Xaml;
using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class XamlSystemPatterns : Case
    {
        public void XamlServicesLoad()
        {
            Pattern.CreateByName(it => 
                XamlServices.Load(it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>()));
        }
        
        public void XamlServicesParse()
        {
            Pattern.CreateBySignature(it => 
                XamlServices.Parse(it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>().Cast<string>()));
        }

        public void XamlServicesTransform()
        {
            Pattern.CreateByName(it => 
                XamlServices.Transform(
                    it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>().Cast<XamlReader>(), 
                    new XamlObjectWriter(new XamlSchemaContext())));
        }

        private void XamlObjectWriterWhat()
        {
            // TODO: check XamlObjectWriter and other XamlWriters
            //var a = new XamlObjectWriter();
        }
    }
}