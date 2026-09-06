using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class SystemUserQueryOptions : UserQueryOptions
{
    public bool IncludeSystemRoles { get; set; }
    public bool IncludeOrganizations { get; set; }
    public bool IncludeOrganizationsRoles { get; set; }

    public SystemUserQueryOptions() { }

    public SystemUserQueryOptions(SystemUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeSystemRoles = options.IncludeSystemRoles;
        IncludeOrganizations = options.IncludeOrganizations;
        IncludeOrganizationsRoles = options.IncludeOrganizationsRoles;
    }

    public override SystemUserQueryOptions Clone()
        => new(this);

    public override SystemUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("includeSystemRoles"))
            IncludeSystemRoles = bool.Parse(request.Query["includeSystemRoles"].ToString());

        if (request.Query.ContainsKey("includeOrganizations"))
            IncludeOrganizations = bool.Parse(request.Query["includeOrganizations"].ToString());

        if (request.Query.ContainsKey("includeOrganizationsRoles"))
            IncludeOrganizationsRoles = bool.Parse(request.Query["includeOrganizationsRoles"].ToString());

        return this;
    }
}
