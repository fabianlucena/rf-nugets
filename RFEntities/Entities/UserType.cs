using System.ComponentModel.DataAnnotations.Schema;

namespace RFEntities.Entities
{
    [Table("UserTypes", Schema = "auth")]
    public sealed class UserType : LocalizableEntity
    {
        public UserType() { }

        public UserType(UserType? userType = null)
            : base(userType)
        {
            if (userType == null)
                return;
        }

        public override UserType Clone()
            => new(this);
    }
}
