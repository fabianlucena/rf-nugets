using RFAuth.Entities;

namespace RFAuth.DTO
{
    public class DeviceDTO(Device device) : DeviceMinDTO(device)
    {
        public DateTime CreatedAt { get; set; } = device.CreatedAt;
    }
}
