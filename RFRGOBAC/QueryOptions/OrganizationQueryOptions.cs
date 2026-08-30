using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class OrganizationQueryOptions : ALocalizableEntityQueryOptions
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

    public override OrganizationQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        return this;
    }
}
