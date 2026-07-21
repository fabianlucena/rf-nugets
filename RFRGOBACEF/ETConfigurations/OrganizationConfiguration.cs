using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFRGOBAC.Entities;

namespace RFRGOBACEF.ETConfigurations;

public class OrganizationConfiguration : CommonEntityConfiguration<Organization>
{
    public override void Configure(EntityTypeBuilder<Organization> entity)
    {
        base.Configure(entity);
    }
}
