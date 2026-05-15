using RFBaseEntities.Entities;

namespace RFBaseIServices.DTO
{
    public class UserMinDTO(User user)
    {
        public Guid Uuid { get; } = user.Uuid;
        public string Username { get; } = user.Username;
        public string DisplayName { get; } = user.DisplayName;
    }
}
