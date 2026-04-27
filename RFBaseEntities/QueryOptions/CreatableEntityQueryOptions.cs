namespace RFBaseEntities.QueryOptions
{
    public class CreatableJoinQueryOptions : JoinQueryOptions
    {
        public bool IncludeCreatedBy { get; set; } = false;
    }
}
