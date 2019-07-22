using System.Windows.Markup;
using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class XamlMarkupPatterns : Case
    {
        public void XamlReaderLoadAsync()
        {
            var reader = new XamlReader();
            Pattern.CreateByName(it =>
                reader.LoadAsync(it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>()));
        }
        
        public void XamlReaderLoad()
        {
            Pattern.CreateByName(it => 
                XamlReader.Load(it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>()));
        }
        
        public void XmlReaderParse()
        {
            Pattern.CreateByName(it => 
                XamlReader.Parse(it.IsPayloadOf<ObjectDataProvider>().Format<Xaml>().Cast<string>()));
        }
    }
}