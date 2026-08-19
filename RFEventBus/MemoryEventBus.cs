namespace RFEventBus;

public class MemoryEventBus
    : IEventBus
{
    private static readonly Dictionary<string, EventHandler[]> EventsHandlers = [];

    public async Task Publish(Event evt)
    {
        if (!EventsHandlers.TryGetValue(evt.Type, out var handlers))
            return;

        var tasks = handlers.Select(handler => handler(evt));
        await Task.WhenAll(tasks);
        return;
    }

    public void Subscribe(string eventType, EventHandler handler)
    {
        if (!EventsHandlers.TryGetValue(eventType, out var handlers))
            handlers = [];

        EventsHandlers[eventType] = [..handlers, handler];
    }
}
