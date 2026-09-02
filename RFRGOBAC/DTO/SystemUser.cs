using RFEntities.Entities;
using RFRBAC.Entities;
using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class SystemUser : User
{
    public string Password { get; set; } = string.Empty;

    public IEnumerable<long> SystemRolesId { get; set; } = [];
    public IEnumerable<Role>? SystemRoles { get; set; }

    public IEnumerable<long> OrganizationsId { get; set; } = [];
    public IEnumerable<Organization>? Organizations { get; set; }

    public IEnumerable<OrganizationRolesId> OrganizationsRolesId { get; set; } = [];
    public IEnumerable<OrganizationRoles>? OrganizationsRoles { get; set; }

    public SystemUser() { }

    public SystemUser(SystemUser? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Password = entity.Password;
        SystemRolesId = entity.SystemRolesId;
        SystemRoles = entity.SystemRoles;
        OrganizationsId = entity.OrganizationsId;
        Organizations = entity.Organizations;
        OrganizationsRolesId = entity.OrganizationsRolesId;
        OrganizationsRoles = entity.OrganizationsRoles;
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
