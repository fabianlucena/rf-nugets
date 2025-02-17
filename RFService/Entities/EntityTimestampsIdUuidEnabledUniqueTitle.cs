using RFService.Libs;

namespace RFService.Entities
{
    [Index(nameof(Title), IsUnique = true)]
    public abstract class EntityTimestampsIdUuidEnabledUniqueTitle
        : EntityTimestampsIdUuidEnabledTitle
    {
    }
}