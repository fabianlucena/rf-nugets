using RFService.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("UsersTypes", Schema = "auth")]
    public class UserType : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}