using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class OrganizationDTO(Organization Organization)
{
    public Guid Uuid { get; } = Organization.Uuid;
    public bool IsActive { get; } = Organization.IsActive;
    public string Name { get; } = Organization.Name;
    public string Title { get; } = Organization.Title;
    public string Description { get; } = Organization.Description;
    public DateTime CreatedAt { get; } = Organization.CreatedAt;
    public DateTime UpdatedAt { get; } = Organization.UpdatedAt;
    public DateTime? DeletedAt { get; } = Organization.DeletedAt;
}
