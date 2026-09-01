using RFRBAC.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class RoleXUserXOrganizationQueryOptions : RoleXUserQueryOptionsBase
{
    public bool IncludeOrganization { get; set; } = false;

    public long? OrganizationId { get; set; }
    public IEnumerable<long>? OrganizationsId { get; set; }

    public RoleXUserXOrganizationQueryOptions() { }

    public RoleXUserXOrganizationQueryOptions(RoleXUserXOrganizationQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeOrganization = options.IncludeOrganization;

        UsersId = options.UsersId;
        OrganizationId = options.OrganizationId;
        OrganizationsId = options.OrganizationsId;
    }

    public override RoleXUserXOrganizationQueryOptions Clone()
        => new(this);
}
