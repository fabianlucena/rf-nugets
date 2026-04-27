namespace RFBaseEntities.QueryOptions
{
    public class CreatableEntityQueryOptions : EntityQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;
    }
}
