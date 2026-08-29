using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class OrganizationQueryOptions : CommonEntityQueryOptions
{
    public OrganizationQueryOptions() { }

    public OrganizationQueryOptions(OrganizationQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override OrganizationQueryOptions Clone()
        => new(this);

    public OrganizationQueryOptions BuildFromRequest(HttpRequest request)
    {
        var options = new OrganizationQueryOptions(this);

        base.BuildFromRequest(request, options);

        return options;
    }
}
