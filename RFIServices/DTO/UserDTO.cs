using RFEntities.Entities;

namespace RFIServices.DTO
{
    public class UserDTO(User user) : UserMinDTO(user)
    {
        public bool IsActive { get; } = user.IsActive;
        public bool CanLogin { get; } = user.CanLogin;
        public DateTime? LastLoginAt { get; } = user.LastLoginAt;

        public DateTime CreatedAt { get; } = user.CreatedAt;
        public DateTime UpdatedAt { get; } = user.UpdatedAt;
        public DateTime? DeletedAt { get; } = user.DeletedAt;

        public UserMinDTO? CreatedBy { get; } = user.CreatedBy != null ? new UserMinDTO(user.CreatedBy) : null;
        public UserMinDTO? UpdatedBy { get; } = user.UpdatedBy != null ? new UserDTO(user.UpdatedBy) : null;
        public UserMinDTO? DeleteBy { get; } = user.DeletedBy != null ? new UserDTO(user.DeletedBy) : null;
    }
}
