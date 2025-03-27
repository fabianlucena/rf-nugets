namespace RFAuth.DTO
{
    public class SessionDTO
    {
        public Guid Uuid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public UserDTO? User { get; set; }

        public DeviceDTO? Device { get; set; }
    }
}
