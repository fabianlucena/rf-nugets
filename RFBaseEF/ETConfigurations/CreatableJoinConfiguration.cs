using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
{
    public class CreatableJoinConfiguration<T>
        : JoinConfiguration<T>
        where T : CreatableJoin
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);
            entity.HasOne(u => u.CreatedBy)
                  .WithMany()
                  .HasForeignKey(u => u.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
