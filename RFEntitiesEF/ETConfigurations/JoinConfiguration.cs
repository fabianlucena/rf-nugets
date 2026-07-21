using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntities.Entities;

namespace RFEntitiesEF.ETConfigurations
{
    public class JoinConfiguration<T>
        : BaseConfiguration<T>
        where T : Join
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);
        }
    }
}
