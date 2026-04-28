using System.ComponentModel.DataAnnotations.Schema;

namespace RFBaseEntities.Entities
{
    [Table("UserTypes", Schema = "auth")]
    public class UserType : LocalizableEntity
    {
    }
}
