using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class OrganizationUserQueryOptions : UserQueryOptions
{
    public OrganizationUserQueryOptions() { }

    public OrganizationUserQueryOptions(OrganizationUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override OrganizationUserQueryOptions Clone()
        => new(this);

    public override OrganizationUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        return this;
    }
}
