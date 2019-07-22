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
            Pattern.CreateBySignature(it =>
                serializer.Deserialize(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>()));
        }
        
        public void DeserializeHeaderHandler()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                serializer.Deserialize(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
                    null));
        }
        
        public void DeserializeMethodResponse()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                serializer.DeserializeMethodResponse(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
                    null,
                    null));
        }
        
        public void UnsafeDeserialize()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                serializer.UnsafeDeserialize(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
                    null));
        }

        public void UnsafeDeserializeMethodResponse()
        {
            var serializer = new BinaryFormatter();
            Pattern.CreateBySignature(it =>
                serializer.UnsafeDeserializeMethodResponse(
                    it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
                    null,
                    null));
        }
    }
}