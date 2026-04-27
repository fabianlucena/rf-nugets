using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFRGCBACEntities.Entities;

namespace RFRGCBACEF.ETConfigurations
{
    public class SessionCompanyConfiguration : NoIdEntityConfiguration<SessionCompany>
    {
        public override void Configure(EntityTypeBuilder<SessionCompany> entity)
        {
            base.Configure(entity);

            entity.HasOne(sc => sc.Session)
                  .WithMany()
                  .HasForeignKey(sc => sc.SessionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sc => sc.Company)
                  .WithMany()
                  .HasForeignKey(sc => sc.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
