using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class PermissionXRoleQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludePermission { get; set; } = false;
        public bool IncludeRole { get; set; } = false;
    }
}
