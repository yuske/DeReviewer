using System.IO;
using System.Text;

namespace DeReviewer.KnowledgeBase
{
    internal class Payload
    {
        public Payload(Stream data)
        {
            Data = data;
        }

        public Payload(string data)
        {
            Data = new MemoryStream(Encoding.UTF8.GetBytes(data));
        }

        public Stream Data { get; }

        public override string ToString()
        {
            var reader = new StreamReader(Data, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}