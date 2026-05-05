using RFRBACEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public sealed class RoleXUserXOrganizationQueryOptions : RoleXUserQueryOptionsBase
    {
        public bool IncludeOrganization { get; set; } = false;

        public long? RoleId { get; set; }
        public long? UserId { get; set; }
        public long? OrganizationId { get; set; }

        public RoleXUserXOrganizationQueryOptions() { }

        public RoleXUserXOrganizationQueryOptions(RoleXUserXOrganizationQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeOrganization = options.IncludeOrganization;

            RoleId = options.RoleId;
            UserId = options.UserId;
            OrganizationId = options.OrganizationId;
        }

        public override RoleXUserXOrganizationQueryOptions Clone()
            => new(this);
    }
}
