using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFRGOBACEntities.Entities;

namespace RFRGOBACEF.ETConfigurations
{
    public class SessionOrganizationConfiguration : NoIdEntityConfiguration<SessionOrganization>
    {
        public override void Configure(EntityTypeBuilder<SessionOrganization> entity)
        {
            base.Configure(entity);

            entity.HasKey(e => new { e.SessionId });

            entity.HasOne(sc => sc.Session)
                  .WithMany()
                  .HasForeignKey(sc => sc.SessionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sc => sc.Organization)
                  .WithMany()
                  .HasForeignKey(sc => sc.OrganizationId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
