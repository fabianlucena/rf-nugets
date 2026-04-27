using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEntities.Entities;

namespace RFBaseEF.ETConfigurations
{
    public class UserConfiguration
        : CommonEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> entity)
        {
            base.Configure(entity);
        }
    }
}
