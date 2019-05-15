using System;
using System.IO;

namespace DeReviewer.KnowledgeBase
{
    internal static class It
    {
        public static T IsPayload<T>(string fileName)
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

            throw new NotImplementedException(
                $"Must add an implementation of payload converting to '{payloadType}' in {typeof(It)}::{nameof(IsPayload)}<T>()");
        }

        private static T Cast<T>(object obj) => (T) obj;
        
        private static string PayloadAsString(string fileName)
        {
            var payload = File.ReadAllText($@"Payloads\{fileName}");
            return payload.Replace("%CMD%", Dsl.PayloadCommand);
        }

        private static TextReader PayloadAsTextReader(string fileName) 
            => new StringReader(PayloadAsString(fileName));
    }
}