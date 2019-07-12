using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class BinaryFormatterPatterns : Case
    {
        public void Deserialize()
        {
            var serializer = new BinaryFormatter();
            Pattern.Create(it =>
                serializer.Deserialize(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>().Cast<Stream>()));
        }
    }
}