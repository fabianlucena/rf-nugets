using RFAuth.Entities;

namespace RFAuth.DTO
{
    public class DeviceMinDTO(Device device)
    {
        public Guid Uuid { get; set; } = device.Uuid;
    }
}
