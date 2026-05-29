using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntities.Entities;

namespace RFEntitiesEF.ETConfigurations
{
    public class CreatableEntityConfiguration<T>
        : EntityConfiguration<T>
        where T : CreatableEntity
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
