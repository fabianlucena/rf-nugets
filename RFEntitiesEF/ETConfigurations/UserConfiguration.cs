using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntities.Entities;

namespace RFEntitiesEF.ETConfigurations
{
    public class UserConfiguration
        : CommonEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> entity)
        {
            base.Configure(entity);

            entity.HasOne(u => u.Type)
                  .WithMany()
                  .HasForeignKey(u => u.TypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
