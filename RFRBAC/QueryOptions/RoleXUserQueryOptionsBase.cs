using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public abstract class RoleXUserQueryOptionsBase : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeUser { get; set; } = false;

        public RoleXUserQueryOptionsBase() { }

        public RoleXUserQueryOptionsBase(RoleXUserQueryOptionsBase? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeRole = options.IncludeRole;
            IncludeUser = options.IncludeUser;
        }
    }
}
