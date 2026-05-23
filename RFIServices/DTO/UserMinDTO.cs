using RFEntities.Entities;

namespace RFIServices.DTO
{
    public class UserMinDTO(User user)
    {
        public Guid Uuid { get; } = user.Uuid;
        public string Username { get; } = user.Username;
        public string DisplayName { get; } = user.DisplayName;
    }
}
