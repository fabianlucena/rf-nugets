namespace RFTransactionLog.DTO
{
    public class LogActionDTO
    {
        public Guid Uuid { get; set; }

        public required string Name { get; set; }
    }
}
