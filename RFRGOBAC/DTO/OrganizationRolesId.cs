namespace RFRGOBAC.DTO;

public class OrganizationRolesId
{
    public long OrganizationId { get; set; }
    public IEnumerable<long> RolesId { get; set; } = [];
}
