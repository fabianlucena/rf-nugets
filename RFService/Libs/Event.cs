namespace RFService.Libs
{
    public class Event
    {
        public string Type { get; set; } = string.Empty;

        public string Entity { get; set; } = string.Empty;

        public object? Data { get; set; } = null;

        public dynamic? Metadata { get; set; } = null;
    }
}
