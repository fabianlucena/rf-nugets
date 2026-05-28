using RFEntities.Entities;

namespace RFIServices.DTO
{
    public class UserTypeMinDTO(UserType userType)
    {
        public Guid Uuid { get; } = userType.Uuid;
        public string Name { get; } = userType.Name;
    }
}
