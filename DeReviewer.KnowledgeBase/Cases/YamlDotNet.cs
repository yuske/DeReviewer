using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class YamlDotNet : Case
    {
        public void MostGenericPattern()
        {
            var deserializer = new Deserializer();
            Pattern.Of<Deserializer>()
                .AssemblyVersionOlderThan(5, 0)
                .Create(() => 
                    deserializer.Deserialize(
                        It.IsPayloadFrom("ObjectDataProvider.yaml").Cast<IParser>(), 
                        typeof(object)));
        }
    }
}