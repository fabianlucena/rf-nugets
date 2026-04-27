using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFAuthEntities.Entities;
using RFBaseEF.ETConfigurations;

namespace RFAuthEF.ETConfigurations
{
    public class SessionConfiguration : CreatableEntityConfiguration<Session>
    {
        public override void Configure(EntityTypeBuilder<Session> entity)
        {
            base.Configure(entity);
        }
    }
}
