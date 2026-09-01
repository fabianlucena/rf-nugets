using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public abstract class RoleXUserQueryOptionsBase : CommonJoinQueryOptions
    {
        public bool IncludeRole { get; set; } = false;
        public bool IncludeUser { get; set; } = false;

        public long? RoleId { get; set; }
        public IEnumerable<long>? RolesId { get; set; }
        public long? UserId { get; set; }
        public IEnumerable<long>? UsersId { get; set; }

        public RoleXUserQueryOptionsBase() { }

        public RoleXUserQueryOptionsBase(RoleXUserQueryOptionsBase? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeRole = options.IncludeRole;
            IncludeUser = options.IncludeUser;

            RoleId = options.RoleId;
            RolesId = options.RolesId;
            UserId = options.UserId;
            UsersId = options.UsersId;
        }
    }
}
