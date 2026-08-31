using RFEntities.Entities;

namespace RFRGOBAC.DTO;

public class OrganizationUser : User
{
    public OrganizationUser() { }

    public OrganizationUser(OrganizationUser? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;
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
