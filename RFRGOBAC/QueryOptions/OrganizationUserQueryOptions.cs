using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class SystemUserQueryOptions : UserQueryOptions
{
    public bool IncludeGlobalRoles { get; set; }
    public bool IncludeOrganizationsRoles { get; set; }

    public SystemUserQueryOptions() { }

    public SystemUserQueryOptions(SystemUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeGlobalRoles = options.IncludeGlobalRoles;
        IncludeOrganizationsRoles = options.IncludeOrganizationsRoles;
    }

    public override SystemUserQueryOptions Clone()
        => new(this);

    public override SystemUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("includeGlobalRoles"))
            IncludeGlobalRoles = bool.Parse(request.Query["includeGlobalRoles"].ToString());

        if (request.Query.ContainsKey("includeOrganizationsRoles"))
            IncludeOrganizationsRoles = bool.Parse(request.Query["includeOrganizationsRoles"].ToString());

        return this;
    }
}
