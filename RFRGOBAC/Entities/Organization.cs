using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBAC.Entities;

[Table("Organizations", Schema = "auth")]
public sealed class Organization : ALocalizableEntity
{
    public string Description { get; set; } = string.Empty;

    public Organization() { }

    public Organization(Organization? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Description = entity.Description;
    }

    public override Organization Clone()
        => new(this);
}
