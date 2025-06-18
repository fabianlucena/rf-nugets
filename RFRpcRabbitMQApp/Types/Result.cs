namespace RFRpcRabbitMQApp.Types
{
    public class Result
    {
        public bool Ok { get; set; } = false;
        public object? Value { get; set; }
    }
}
