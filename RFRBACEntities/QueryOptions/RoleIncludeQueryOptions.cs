using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class RoleIncludeQueryOptions : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeInclude { get; set; } = false;

        public RoleIncludeQueryOptions() { }

        public RoleIncludeQueryOptions(RoleIncludeQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeRole = options.IncludeRole;
            IncludeInclude = options.IncludeInclude;
        }

        public override RoleIncludeQueryOptions Clone()
            => new(this);
    }
}
