using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class RoleXUserQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeUser { get; set; } = false;

        public RoleXUserQueryOptions() { }

        public RoleXUserQueryOptions(RoleXUserQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeRole = options.IncludeRole;
            IncludeUser = options.IncludeUser;
        }

        public override RoleXUserQueryOptions Clone()
            => new(this);
    }
}
