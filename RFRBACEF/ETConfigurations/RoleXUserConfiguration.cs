using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFRBAC.Entities;

namespace RFRBACEF.ETConfigurations;

public class RoleXUserConfiguration
    : CommonJoinConfiguration<RoleXUser>
{
    public override void Configure(EntityTypeBuilder<RoleXUser> entity)
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
    }
}
