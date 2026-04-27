using System.ComponentModel.DataAnnotations.Schema;

namespace RFBaseEntities.Entities
{
    [Table("UsersTypes", Schema = "auth")]
    public class UserType : LocalizableEntity
    {
    }
}
