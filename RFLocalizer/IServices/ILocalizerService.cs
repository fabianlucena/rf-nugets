namespace RFLocalizer.IServices
{
    public interface ILocalizerService
    {
        string this[string text] { get; }
    }
}
