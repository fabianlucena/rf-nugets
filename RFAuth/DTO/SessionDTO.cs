using RFAuth.Entities;
using RFIServices.DTO;

namespace RFAuth.DTO
{
    public class SessionDTO(Session session) : SessionMinDTO(session)
    {
        public Guid Uuid { get; set; } = session.Uuid;

        public DateTime CreatedAt { get; set; } = session.CreatedAt;
        public DateTime ExpireAt { get; set; } = session.ExpireAt;
        public DateTime LastUsedAt { get; set; } = session.LastUsedAt;
        public DateTime? ClosedAt { get; set; } = session.ClosedAt;

        public UserMinDTO? User { get; set; } = session.User != null ? new UserMinDTO(session.User) : null;
        public DeviceMinDTO? Device { get; set; } = session.Device != null ? new DeviceMinDTO(session.Device) : null;
    }
}
