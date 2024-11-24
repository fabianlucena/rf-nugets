using RFService.Exceptions;
using RFService.IServices;

namespace RFService.Libs
{
    public class EventBus
        : IEventBus
    {
        private readonly Dictionary<string, Dictionary<string, List<Listener>>> _listeners = [];

        public bool AddListener(
            string eventType,
            string entity,
            Listener listener
        )
        {
            if (string.IsNullOrWhiteSpace(eventType))
                throw new NullEventTypeException();

            eventType = eventType.Trim().ToLower();
            if (!_listeners.TryGetValue(eventType, out Dictionary<string, List<Listener>>? entitiesListeners))
            {
                entitiesListeners = [];
                _listeners[eventType] = entitiesListeners;
            }

            entity = entity.Trim();
            if (!entitiesListeners.TryGetValue(entity, out List<Listener>? listeners))
            {
                listeners = [];
                entitiesListeners[entity] = listeners;
            }


            listeners.Add(listener);

            return true;
        }

        public async Task FireAsync(
            string eventType,
            string entity,
            EventData? data = null)
        {
            eventType = eventType.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(eventType))
                return;


            if (!_listeners.TryGetValue(eventType, out Dictionary<string, List<Listener>>? entitiesListeners))
                return;

            entity = entity.Trim();
            if (!entitiesListeners.TryGetValue(entity, out List<Listener>? listeners))
                return;

            foreach (var listener in listeners)
            {
                await listener(new Event
                {
                    Type = eventType,
                    Entity = entity,
                    Data = data,
                });
            }
        }
    }
}
