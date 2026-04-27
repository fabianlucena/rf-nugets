using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class RoleIncludeQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeInclude { get; set; } = false;
    }
}
