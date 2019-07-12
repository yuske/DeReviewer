using System;
using System.IO;

namespace DeReviewer.KnowledgeBase
{
    internal static class PayloadCast
    {
        public static T Cast<T>(this Payload payload)
        {
            T result;
            
            if (Match(() => payload.ToString(), out result)) return result;
            if (Match(() => new StringReader(payload.ToString()), out result)) return result;    // for TextReader 
            if (Match(() => new YamlDotNet.Core.Parser(payload.Cast<TextReader>()), out result)) return result;
            if (Match(() => System.Xml.XmlReader.Create(payload.Cast<TextReader>()), out result)) return result;
            
            throw new NotImplementedException(
                $"Must add an implementation of payload converting to '{typeof(T)}' in {typeof(PayloadCast)}::{nameof(Cast)}<T>()");
        }

        public static string SaveAsTempFile(this Payload payload)
        {
            // create temp file with payload content and return file name
            // life-time of the file should depend on Context life-time
            throw new NotImplementedException();
        }

        private static bool Match<TResult, TPattern>(Func<TPattern> converter, out TResult result)
            //where TResult : class
            //where TPattern : class
        {
            if (typeof(TResult).IsAssignableFrom(typeof(TPattern)))
            {
                result = (TResult) (object) converter();
                return true;
            }

            result = default(TResult);
            return false;
        }
    }
}