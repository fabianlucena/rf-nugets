namespace RFRGOBAC.DTO;

public class OrganizationRolesId
{
    public long Id { get; set; }
    public IEnumerable<long> RolesId { get; set; } = [];
}
