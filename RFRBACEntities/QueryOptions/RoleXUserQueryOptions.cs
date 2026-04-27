using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class RoleXUserQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeUser { get; set; } = false;
    }
}
