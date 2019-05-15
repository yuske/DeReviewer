using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class YamlDotNet
    {
        public void MostGenericPattern()
        {
            var deserializer = new Deserializer();
            Pattern.Of<Deserializer>()
                .AssemblyVersionOlderThan(5, 0)
                .Create(() => deserializer.Deserialize(It.IsPayload<IParser>("ObjectDataProvider.yaml"), typeof(object)));
        }
    }
}