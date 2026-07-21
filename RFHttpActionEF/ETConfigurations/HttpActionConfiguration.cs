using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFHttpAction.Entities;

namespace RFHttpActionEF.ETConfigurations;

public class HttpActionConfiguration
    : AuditableEntityConfiguration<HttpAction>
{
    public override void Configure(EntityTypeBuilder<HttpAction> entity)
    {
        base.Configure(entity);
    }
}
