using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFAuth.Entities;
using RFEntitiesEF.ETConfigurations;

namespace RFAuthEF.ETConfigurations
{
    public class DeviceConfiguration : BaseConfiguration<Device>
    {
        public override void Configure(EntityTypeBuilder<Device> entity)
        {
            base.Configure(entity);
        }
    }
}
