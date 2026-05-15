using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
{
    public class AuditableEntityConfiguration<T>
        : CreatableEntityConfiguration<T>
        where T : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);

            entity.HasOne(u => u.UpdatedBy)
                  .WithMany()
                  .HasForeignKey(u => u.UpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
