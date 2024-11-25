using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Permissions", Schema = "auth")]
    [Index(nameof(Name), IsUnique = true)]
    public class Permission
        : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}