using System;
using System.IO;

namespace DeReviewer.KnowledgeBase
{
    internal class It
    {
        private readonly Context context;

        public It(Context context)
        {
            this.context = context;
        }
    
        public T IsPayload<T>(string fileName)
        {
            var payloadType = typeof(T);
            if (payloadType == typeof(string))
            {
                return Cast<T>(PayloadAsString(fileName));
            }

            if (payloadType.IsAssignableFrom(typeof(YamlDotNet.Core.Parser)))
            {
                return Cast<T>(new YamlDotNet.Core.Parser(PayloadAsTextReader(fileName)));
            }

            if (payloadType.IsAssignableFrom(typeof(System.Xml.XmlReader)))
            {
                return Cast<T>(System.Xml.XmlReader.Create(PayloadAsTextReader(fileName)));
            }

            throw new NotImplementedException(
                $"Must add an implementation of payload converting to '{payloadType}' in {typeof(It)}::{nameof(IsPayload)}<T>()");
        }

        private T Cast<T>(object obj) => (T) obj;
        
        private string PayloadAsString(string fileName)
        {
            var payload = File.ReadAllText($@"Payloads\{fileName}");
            return payload.Replace("%CMD%", context.PayloadCommand);
        }

        private TextReader PayloadAsTextReader(string fileName) 
            => new StringReader(PayloadAsString(fileName));
    }
}