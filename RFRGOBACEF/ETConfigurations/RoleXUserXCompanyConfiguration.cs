using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFRGCBACEntities.Entities;

namespace RFRGCBACEF.ETConfigurations
{
    public class RoleXUserXCompanyConfiguration
        : CommonJoinConfiguration<RoleXUserXCompany>
    {
        public override void Configure(EntityTypeBuilder<RoleXUserXCompany> entity)
        {
            base.Configure(entity);

            entity.HasNoKey();

            entity.HasOne(u => u.Role)
                  .WithMany()
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.User)
                  .WithMany()
                  .HasForeignKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Company)
                  .WithMany()
                  .HasForeignKey(u => u.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
