using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
{
    public class BaseConfiguration<T>
        : IEntityTypeConfiguration<T>
        where T : Base
    {
        public virtual void Configure(EntityTypeBuilder<T> entity)
        {
        }
    }
}
