using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFPermissionsEntities.Entities;

namespace RFPermissionsEF.ETConfigurations
{
    public class PermissionConfiguration
        : ImmutableEntityConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> entity)
        {
            base.Configure(entity);
        }
    }
}
