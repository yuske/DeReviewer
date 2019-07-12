using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class YamlDotNetPatterns : Case
    {
        public void MostGenericPattern()
        {
            var deserializer = new Deserializer();
            Pattern.Of<Deserializer>()
                .AssemblyVersionOlderThan(5, 0)
                .CreateBySignature(it => 
                    deserializer.Deserialize(
                        it.IsPayloadFrom("ObjectDataProvider.yaml").Cast<IParser>(), 
                        typeof(object)));
        }
    }
}