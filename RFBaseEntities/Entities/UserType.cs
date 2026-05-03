using RFBaseEntities.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFBaseEntities.Entities
{
    [Table("UserTypes", Schema = "auth")]
    public class UserType : LocalizableEntity
    {
        public UserType() { }

        public UserType(UserType _) { }

        public override UserType Clone()
        {
            return new UserType(this);
        }
    }
}
