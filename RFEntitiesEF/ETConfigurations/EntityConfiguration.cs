using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntities.Entities;

namespace RFEntitiesEF.ETConfigurations
{
    public class EntityConfiguration<T>
        : BaseConfiguration<T>
        where T : Entity
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);
        }
    }
}
