using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public class PermissionXRoleQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludePermission { get; set; } = false;
        public bool IncludeRole { get; set; } = false;

        public PermissionXRoleQueryOptions() { }

        public PermissionXRoleQueryOptions(PermissionXRoleQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override PermissionXRoleQueryOptions Clone()
            => new(this);
    }
}
