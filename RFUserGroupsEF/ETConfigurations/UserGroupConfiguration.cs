using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFEntitiesEF.ETConfigurations;
using RFUserGroups.Entities;

namespace RFUserGroupsEF.ETConfigurations;

public class UserGroupConfiguration
    : CommonJoinConfiguration<UserGroup>
{
    public override void Configure(EntityTypeBuilder<UserGroup> entity)
    {
        base.Configure(entity);

        entity.HasOne(ug => ug.User)
              .WithMany()
              .HasForeignKey(ug => ug.UserId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(ug => ug.Group)
              .WithMany()
              .HasForeignKey(ug => ug.GroupId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
