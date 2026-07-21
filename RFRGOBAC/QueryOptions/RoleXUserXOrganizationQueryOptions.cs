using RFRBAC.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class RoleXUserXOrganizationQueryOptions : RoleXUserQueryOptionsBase
{
    public bool IncludeOrganization { get; set; } = false;

    public long? OrganizationId { get; set; }

    public RoleXUserXOrganizationQueryOptions() { }

    public RoleXUserXOrganizationQueryOptions(RoleXUserXOrganizationQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeOrganization = options.IncludeOrganization;

        UserIds = options.UserIds;
        OrganizationId = options.OrganizationId;
    }

    public override RoleXUserXOrganizationQueryOptions Clone()
        => new(this);
}
