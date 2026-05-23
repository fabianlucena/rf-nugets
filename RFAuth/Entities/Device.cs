using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Devices", Schema = "auth")]
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
