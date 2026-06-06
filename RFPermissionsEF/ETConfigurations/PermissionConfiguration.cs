using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFPermissions.Entities;

namespace RFPermissionsEF.ETConfigurations;

public class PermissionConfiguration
    : ImmutableEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> entity)
    {
        base.Configure(entity);
    }
}
