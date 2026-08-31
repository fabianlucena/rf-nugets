using Microsoft.AspNetCore.Http;

namespace RFRGOBAC.QueryOptions;

public class SystemUserQueryOptions : OrganizationUserQueryOptions
{
    public SystemUserQueryOptions() { }

    public SystemUserQueryOptions(SystemUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override SystemUserQueryOptions Clone()
        => new(this);

    public override SystemUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        return this;
    }
}
