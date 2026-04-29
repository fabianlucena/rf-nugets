using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFBaseEF.ETConfigurations;
using RFRGCBACEntities.Entities;

namespace RFRGCBACEF.ETConfigurations
{
    public class CompanyConfiguration : CommonEntityConfiguration<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> entity)
        {
            base.Configure(entity);
        }
    }
}
