namespace RFEventBus;

public delegate Task EventHandler(Event evt);

public interface IEventBus
{
    void Subscribe(string eventType, EventHandler handler);
    Task Publish(Event evt);
}
