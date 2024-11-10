using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles", Schema = "auth")]
    [Index(nameof(Name), IsUnique = true)]
    public class Role : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}