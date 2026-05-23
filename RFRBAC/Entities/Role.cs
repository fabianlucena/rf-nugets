using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles", Schema = "auth")]
    public sealed class Role : LocalizableEntity
    {
        public Role() { }

        public Role(Role? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;
        }

        public override Role Clone()
            => new(this);
    }
}
