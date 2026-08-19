namespace RFEventBus;

[AttributeUsage(AttributeTargets.Method)]
public class EventHandlerAttribute(string type = "") : Attribute
{
    public string Type { get; set; } = type;
}
