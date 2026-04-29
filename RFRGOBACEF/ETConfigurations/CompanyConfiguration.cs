using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFRGOBACEntities.Entities;

namespace RFRGOBACEF.ETConfigurations
{
    public class OrganizationConfiguration : CommonEntityConfiguration<Organization>
    {
        public override void Configure(EntityTypeBuilder<Organization> entity)
        {
            base.Configure(entity);
        }
    }
}
