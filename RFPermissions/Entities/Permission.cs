using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFPermissions.Entities
{
    [Table("Permissions", Schema = "auth")]
    public sealed class Permission : ImmutableEntity
    {
        public string Name { get; set; } = string.Empty;

        public Permission() { }

        public Permission(Permission? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            Name = entity.Name;
        }

        public override Permission Clone()
            => new(this);
    }
}
