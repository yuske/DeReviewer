using DeReviewer.KnowledgeBase.Formatters;
using DeReviewer.KnowledgeBase.Gadgets;
using System.Runtime.Serialization.Formatters.Binary;

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

    public void DeserializeTextFormattingRunProperties()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.Deserialize(
          it.IsPayloadOf<TextFormattingRunProperties>().Format<Binary>()));
    }

    public void DeserializeHeaderHandlerTypeConfuseDelegate()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
          serializer.Deserialize(
              it.IsPayloadOf<TypeConfuseDelegate>().Format<Binary>(),
              null));
    }

    public void DeserializeHeaderHandlerTextFormattingRunProperties()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.Deserialize(
          it.IsPayloadOf<TextFormattingRunProperties>().Format<Binary>(),
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

    public void DeserializeMethodResponseTextFormattingRunProperties()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.DeserializeMethodResponse(
          it.IsPayloadOf<TextFormattingRunProperties>().Format<Binary>(),
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

    public void UnsafeDeserializeTextFormattingRunProperties()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.UnsafeDeserialize(
          it.IsPayloadOf<TextFormattingRunProperties>().Format<Binary>(),
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

    public void UnsafeDeserializeMethodResponseTextFormattingRunProperties()
    {
      var serializer = new BinaryFormatter();
      Pattern.CreateBySignature(it =>
        serializer.UnsafeDeserializeMethodResponse(
          it.IsPayloadOf<TextFormattingRunProperties>().Format<Binary>(),
          null,
          null));
    }
  }
}