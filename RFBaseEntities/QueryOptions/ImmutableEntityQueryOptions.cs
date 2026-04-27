namespace RFBaseEntities.QueryOptions
{
    public class ImmutableEntityQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;
    }
}
