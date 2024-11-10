using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Permissions", Schema = "auth")]
    [Index(nameof(Name), IsUnique = true)]
    public class Permission : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}