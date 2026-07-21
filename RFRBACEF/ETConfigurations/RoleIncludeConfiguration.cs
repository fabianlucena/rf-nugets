using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFRBAC.Entities;

namespace RFRBACEF.ETConfigurations
{
    public class RoleIncludeConfiguration
        : CommonJoinConfiguration<RoleInclude>
    {
        public override void Configure(EntityTypeBuilder<RoleInclude> entity)
        {
            base.Configure(entity);

            entity.HasOne(u => u.Role)
                  .WithMany()
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Include)
                  .WithMany()
                  .HasForeignKey(u => u.IncludeId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
