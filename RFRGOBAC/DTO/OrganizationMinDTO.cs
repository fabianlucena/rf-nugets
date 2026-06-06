using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class OrganizationMinDTO(Organization Organization)
{
    public Guid Uuid { get; } = Organization.Uuid;
    public string Name { get; } = Organization.Name;
    public string Title { get; } = Organization.Title;
}
