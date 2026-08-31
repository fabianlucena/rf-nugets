using RFEntities.Entities;
using RFRBAC.Entities;

namespace RFRGOBAC.DTO;

public class SystemUser : User
{
    public IEnumerable<long> GlobalRolesId { get; set; } = [];
    public IEnumerable<Role>? GlobalRoles { get; set; }

    public IEnumerable<OrganizationRolesId> OrganizationsRolesId { get; set; } = [];
    public IEnumerable<OrganizationRoles>? OrganizationsRoles { get; set; }

    public SystemUser() { }

    public SystemUser(SystemUser? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        GlobalRolesId = entity.GlobalRolesId;
        OrganizationsRolesId = entity.OrganizationsRolesId;
    }

    public SystemUser(User? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;
    }

    public override SystemUser Clone()
        => new(this);
}
