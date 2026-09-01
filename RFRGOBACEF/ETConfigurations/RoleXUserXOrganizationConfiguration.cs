using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFRGOBAC.Entities;

namespace RFRGOBACEF.ETConfigurations;

public class RoleXUserXOrganizationConfiguration
    : CommonJoinConfiguration<RoleXUserXOrganization>
{
    public override void Configure(EntityTypeBuilder<RoleXUserXOrganization> entity)
    {
        base.Configure(entity);

        entity.HasKey(x => new { x.RoleId, x.UserId, x.OrganizationId });

        entity.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(u => u.Organization)
            .WithMany()
            .HasForeignKey(u => u.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
