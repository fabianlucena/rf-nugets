using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class SystemUserQueryOptions : UserQueryOptions
{
    public bool IncludeGlobalRoles { get; set; }

    public SystemUserQueryOptions() { }

    public SystemUserQueryOptions(SystemUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeGlobalRoles = options.IncludeGlobalRoles;
    }

    public override SystemUserQueryOptions Clone()
        => new(this);

    public override SystemUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("includeGlobalRoles"))
            IncludeGlobalRoles = bool.Parse(request.Query["includeGlobalRoles"].ToString());

        return this;
    }
}
