using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFHttpAction.Entities;

namespace RFHttpActionEF.ETConfigurations;

public class HttpActionTypeConfiguration
    : AuditableEntityConfiguration<HttpActionType>
{
    public override void Configure(EntityTypeBuilder<HttpActionType> entity)
    {
        base.Configure(entity);
    }
}
