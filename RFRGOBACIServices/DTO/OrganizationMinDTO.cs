using RFRGOBACEntities.Entities;

namespace RFRGOBACIServices.DTO
{
    public class OrganizationMinDTO(Organization Organization)
    {
        public Guid Uuid { get; } = Organization.Uuid;
        public string Name { get; } = Organization.Name;
        public string Title { get; } = Organization.Title;
        public string Description { get; } = Organization.Description;
    }
}
