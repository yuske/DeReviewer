using System.IO;
using System.Xml.Serialization;
using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;

namespace DeReviewer.KnowledgeBase.Cases
{
    public sealed class XmlSerializerPatterns : Case
    {
        public void Deserialize()
        {
            var serializer = new XmlSerializer(typeof(object));
            Pattern.CreateBySignature(it =>
                serializer.Deserialize(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>().Cast<TextReader>()));
        }
    }
}