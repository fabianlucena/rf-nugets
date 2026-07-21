using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Devices", Schema = "auth")]
    [Index(nameof(Token), IsUnique = true)]
    public sealed class Device : CreatableEntity
    {
        public string Token { get; set; } = string.Empty;

        public Device() { }

        public Device(Device? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            Token = entity.Token;
        }

        public override Device Clone()
            => new(this);
    }
}
