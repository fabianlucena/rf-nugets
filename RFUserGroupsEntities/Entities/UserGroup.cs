using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserGroupsEntities.Entities
{
    [Table("UserGroups", Schema = "auth")]
    public sealed class UserGroup : CommonJoin
    {
        public long UserId { get; set; }
        public User? User { get; set; }

        public long GroupId { get; set; }
        public User? Group { get; set; }

        public UserGroup() { }

        public UserGroup(UserGroup? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            UserId = entity.UserId;
            User = entity.User;

            GroupId = entity.GroupId;
            Group = entity.Group;
        }

        public override UserGroup Clone()
            => new(this);
    }
}
