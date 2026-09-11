using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public abstract class ACommonEntityQueryOptions : CommonEntityQueryOptions
{
    public bool IncludeInactive { get; set; } = false;

    public ACommonEntityQueryOptions() { }

    public ACommonEntityQueryOptions(ACommonEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeInactive = options.IncludeInactive;
    }

    public override CommonEntityQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeInactive = GetBoolFromRequest(request, "includeInactive");

        return this;
    }
}
