using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
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
