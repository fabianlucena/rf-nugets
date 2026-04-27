using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFAuthEntities.Entities;
using RFBaseEF.ETConfigurations;

namespace RFAuthEF.ETConfigurations
{
    public class UserPasswordConfiguration : NoIdEntityConfiguration<UserPassword>
    {
        public override void Configure(EntityTypeBuilder<UserPassword> entity)
        {
            base.Configure(entity);

            entity.HasKey(e => new { e.UserId });

            entity.HasOne(sc => sc.User)
                  .WithMany()
                  .HasForeignKey(sc => sc.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
