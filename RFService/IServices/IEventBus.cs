global using Listener = System.Func<RFService.Libs.Event, System.Threading.Tasks.Task>;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IEventBus
    {
        bool AddListener(string eventType, string entity, Listener listener);

        bool AddListener(string eventType, Listener listener)
            => AddListener(eventType, "", listener);

        Task FireAsync(string eventType, string entity, EventData? data);

        Task FireAsync(string eventType, EventData? data = null)
            => FireAsync(eventType, "", data);
    }
}
