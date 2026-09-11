using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class ANominableEntityQueryOptions : NominableEntityQueryOptions
{
    public bool IncludeInactive { get; set; } = false;

    public ANominableEntityQueryOptions() { }

    public ANominableEntityQueryOptions(ANominableEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeInactive = options.IncludeInactive;
    }

    public override ANominableEntityQueryOptions Clone()
        => new(this);

    public override ANominableEntityQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeInactive = GetBoolFromRequest(request, "includeInactive");

        return this;
    }
}
