using RFRBACEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public sealed class RoleXUserXOrganizationQueryOptions : RoleXUserQueryOptionsBase
    {
        public bool IncludeOrganization { get; set; } = false;

        public RoleXUserXOrganizationQueryOptions() { }

        public RoleXUserXOrganizationQueryOptions(RoleXUserXOrganizationQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeOrganization = options.IncludeOrganization;
        }

        public override RoleXUserXOrganizationQueryOptions Clone()
            => new(this);
    }
}
