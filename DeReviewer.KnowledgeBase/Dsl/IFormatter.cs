namespace DeReviewer.KnowledgeBase
{
    internal interface IFormatter
    {
        Payload GeneratePayload(object gadget);
    }
}