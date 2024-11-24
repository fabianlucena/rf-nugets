namespace RFService.Libs
{
    public class Event
    {
        public string Type { get; set; } = string.Empty;

        public string Entity { get; set; } = string.Empty;

        public EventData? Data { get; set; } = null;
    }
}
