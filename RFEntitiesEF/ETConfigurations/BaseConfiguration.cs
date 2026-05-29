using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntities.Entities;

namespace RFEntitiesEF.ETConfigurations
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
