using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles", Schema = "auth")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class Role : LocalizableEntity
    {
        [Required]
        public bool IsSelectable { get; set; } = false;

        public Role() { }

        public Role(Role? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            IsSelectable = entity.IsSelectable;
        }

        public override Role Clone()
            => new(this);
    }
}
