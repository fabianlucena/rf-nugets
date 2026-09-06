using RFEntities.Entities;
using RFRBAC.Entities;

namespace RFRGOBAC.DTO;

public class OrganizationUser : User
{
    public string Password { get; set; } = string.Empty;

    public IEnumerable<long> RolesId { get; set; } = [];
    public IEnumerable<Role>? Roles { get; set; }

    public OrganizationUser() { }

    public OrganizationUser(OrganizationUser? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Password = entity.Password;
        RolesId = entity.RolesId;
        Roles = entity.Roles;
    }

    public OrganizationUser(User? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;
    }

    public override OrganizationUser Clone()
        => new(this);
}
