namespace RFAuth.DTO
{
    public class DeviceDTO
    {
        public Guid Uuid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
