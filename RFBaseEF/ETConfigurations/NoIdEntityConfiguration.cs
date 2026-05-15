using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
{
    public class NoIdEntityConfiguration<T>
        : BaseConfiguration<T>
        where T : NoIdEntity
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);

            entity.HasNoKey();

            entity.HasOne(u => u.CreatedBy)
                  .WithMany()
                  .HasForeignKey(u => u.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.UpdatedBy)
                  .WithMany()
                  .HasForeignKey(u => u.UpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.DeletedBy)
                  .WithMany()
                  .HasForeignKey(u => u.DeletedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
