namespace RFEventBus;

public class Event(string type, object? data = null)
{
    public string Type { get; set; } = type;
    public object? Data { get; set; } = data;
}
