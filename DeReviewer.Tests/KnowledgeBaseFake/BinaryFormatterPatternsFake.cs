using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DeReviewer.KnowledgeBase;
using DeReviewer.KnowledgeBase.Formatters;

namespace DeReviewer.Tests.KnowledgeBaseFake
{
    internal class BinaryFormatterPatternsFake: Case
    {
        public void OnlyPayloadGenerationCall()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                it.IsPayloadOf<TypeConfuseDelegateFake>().Format<Binary>());
        }
        
        public void DeserializeCall()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                serializer.Deserialize(
                    it.IsPayloadOf<TypeConfuseDelegateFake>().Format<Binary>().Cast<Stream>()));
        }
    }
}