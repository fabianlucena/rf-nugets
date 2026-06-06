using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBAC.Entities;

[Table("Organizations", Schema = "auth")]
public sealed class Organization : LocalizableEntity
{
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;

    public Organization() { }

    public Organization(Organization? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
        Description = entity.Description;
    }

    public override Organization Clone()
        => new(this);
}
