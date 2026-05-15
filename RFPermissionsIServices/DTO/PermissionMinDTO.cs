using RFPermissionsEntities.Entities;

namespace RFPermissionsIServices.DTO
{
    public class PermissionMinDTO(Permission permission)
    {
        public Guid Uuid { get; } = permission.Uuid;
        public string Name { get; } = permission.Name;
    }
}
