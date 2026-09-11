using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class ANominableOwnedEntityQueryOptions : NominableOwnedEntityQueryOptions
{
    public bool IncludeInactive { get; set; } = false;

    public ANominableOwnedEntityQueryOptions() { }

    public ANominableOwnedEntityQueryOptions(ANominableOwnedEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeInactive = options.IncludeInactive;
    }

    public override ANominableOwnedEntityQueryOptions Clone()
        => new(this);

    public override ANominableOwnedEntityQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeInactive = GetBoolFromRequest(request, "includeInactive");

        return this;
    }
}
