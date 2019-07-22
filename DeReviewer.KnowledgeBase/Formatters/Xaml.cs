using System.Windows.Markup;

namespace DeReviewer.KnowledgeBase.Formatters
{
    internal class Xaml : IFormatter
    {
        public Payload GeneratePayload(object gadget) => new Payload(XamlWriter.Save(gadget));
    }
}