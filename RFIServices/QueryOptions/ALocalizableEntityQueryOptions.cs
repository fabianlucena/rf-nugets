using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public abstract class ALocalizableEntityQueryOptions : LocalizableEntityQueryOptions
{
    public bool IncludeInactive { get; set; } = false;

    public ALocalizableEntityQueryOptions() { }

    public ALocalizableEntityQueryOptions(ALocalizableEntityQueryOptions? options)
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
