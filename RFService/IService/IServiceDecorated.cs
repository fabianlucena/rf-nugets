namespace RFService.IService
{
    public interface IServiceDecorated
    {
        Task<IDictionary<string, object>?> DecorateAsync(object data, IDictionary<string, object>? property, string name);
    }
}
