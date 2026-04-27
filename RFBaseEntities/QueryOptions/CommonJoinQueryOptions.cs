namespace RFBaseEntities.QueryOptions
{
    public class CommonJoinQueryOptions : BaseQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeDeletedBy { get; set; } = false;
    }
}
