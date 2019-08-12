using System.Runtime.Serialization.Formatters.Binary;
using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;

namespace DeReviewer.KnowledgeBase.Cases
{
  public class BinaryFormatterPatterns : Case
  {
    public void DeserializeTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.Deserialize(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>()));
    }

    public void DeserializePsObject()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.Deserialize(
          it.IsPayloadOf<PsObject>().Format<Binary>()));
    }

    public void DeserializeHeaderHandlerTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.Deserialize(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
              null));
    }

    public void DeserializeHeaderHandlerPsObject()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
            serializer.Deserialize(
              it.IsPayloadOf<PsObject>().Format<Binary>(),
              null));
    }

    public void DeserializeMethodResponseTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.DeserializeMethodResponse(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
              null,
              null));
    }

    public void DeserializeMethodResponsePsObject()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.DeserializeMethodResponse(
          it.IsPayloadOf<PsObject>().Format<Binary>(),
          null,
          null));
    }

    public void UnsafeDeserializeTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.UnsafeDeserialize(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
              null));
    }

    public void UnsafeDeserializePsObject()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.UnsafeDeserialize(
          it.IsPayloadOf<PsObject>().Format<Binary>(),
          null));
    }

    public void UnsafeDeserializeMethodResponseTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.UnsafeDeserializeMethodResponse(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
              null,
              null));
    }

    public void UnsafeDeserializeMethodResponsePsObject()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.UnsafeDeserializeMethodResponse(
          it.IsPayloadOf<PsObject>().Format<Binary>(),
          null,
          null));
    }
  }
}