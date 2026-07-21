using RFRBAC.Entities;

namespace RFRBAC.DTO
{
    public class RoleMinDTO(Role role)
    {
        public Guid Uuid { get; } = role.Uuid;
        public string Name { get; } = role.Name;
        public string Title { get; } = role.Title;
    }
}
