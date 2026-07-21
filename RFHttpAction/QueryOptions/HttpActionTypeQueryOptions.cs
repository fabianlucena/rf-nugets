using RFIServices.QueryOptions;

namespace RFHttpAction.QueryOptions;

public class HttpActionTypeQueryOptions : LocalizableEntityQueryOptions
{
    public HttpActionTypeQueryOptions() { }

    public HttpActionTypeQueryOptions(HttpActionTypeQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override HttpActionTypeQueryOptions Clone()
        => new(this);
}
