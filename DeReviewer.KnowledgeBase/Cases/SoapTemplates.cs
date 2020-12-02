using System.Runtime.Serialization.Formatters.Soap;

namespace DeReviewer.KnowledgeBase.Cases
{
    public sealed class SoapTemplates : Case
    {
        public void Deserialization()
        {
            var serializer = new SoapFormatter();
            Pattern.CreateByName(it =>
                serializer.Deserialize(null));
        }
    }
}