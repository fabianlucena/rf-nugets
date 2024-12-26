namespace RFLocalizer.IServices
{
    public interface ILocalizerContextService
    {
        ILocalizerService this[string context] { get; }
    }
}
