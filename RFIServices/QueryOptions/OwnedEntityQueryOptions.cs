using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class OwnedEntityQueryOptions : CommonEntityQueryOptions
{
    public bool? Mine { get; set; } = null;

    public OwnedEntityQueryOptions() { }

    public OwnedEntityQueryOptions(OwnedEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Mine = options.Mine;
    }

    public override OwnedEntityQueryOptions Clone()
        => new(this);

    public override OwnedEntityQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        Mine = GetNullableBoolFromRequest(request, "mine", Mine);

        return this;
    }
}
