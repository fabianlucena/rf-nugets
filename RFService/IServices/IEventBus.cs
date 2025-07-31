namespace RFService.IServices
{
    public interface IEventBus
    {
        bool AddListener(string eventType, string entity, Listener listener);

        bool RemoveListener(string eventType, string entity, Listener listener);

        bool AddListener(string eventType, Listener listener)
            => AddListener(eventType, "", listener);

        bool RemvoeListener(string eventType, Listener listener)
            => RemoveListener(eventType, "", listener);

        Task FireAsync(string eventType, string entity, object? data, dynamic? metadata = null);

        Task FireAsync(string eventType, object? data = null, dynamic? metadata = null)
            => FireAsync(eventType, "", data, metadata);
    }
}
