using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public abstract class RoleXUserQueryOptionsBase : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeUser { get; set; } = false;

        public long? RoleId { get; set; }
        public IEnumerable<long>? RoleIds { get; set; }
        public long? UserId { get; set; }
        public IEnumerable<long>? UserIds { get; set; }

        public RoleXUserQueryOptionsBase() { }

        public RoleXUserQueryOptionsBase(RoleXUserQueryOptionsBase? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeRole = options.IncludeRole;
            IncludeUser = options.IncludeUser;

            RoleId = options.RoleId;
            RoleIds = options.RoleIds;
            UserId = options.UserId;
            UserIds = options.UserIds;
        }
    }
}
