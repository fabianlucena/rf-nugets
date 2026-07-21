using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFUserEmailVerified.Entities;

namespace RFUserEmailVerifiedEF.ETConfigurations;

public class UserEmailVerifiedConfiguration
    : CreatableEntityConfiguration<UserEmailVerified>
{
    public override void Configure(EntityTypeBuilder<UserEmailVerified> entity)
    {
        base.Configure(entity);
    }
}
