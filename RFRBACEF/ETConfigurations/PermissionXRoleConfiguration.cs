using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFRBAC.Entities;

namespace RFRBACEF.ETConfigurations
{
    public class PermissionXRoleConfiguration
        : CommonJoinConfiguration<PermissionXRole>
    {
        public override void Configure(EntityTypeBuilder<PermissionXRole> entity)
        {
            base.Configure(entity);

            entity.HasKey(x => new { x.PermissionId, x.RoleId });

            entity.HasOne(u => u.Permission)
                  .WithMany()
                  .HasForeignKey(u => u.PermissionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Role)
                  .WithMany()
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
