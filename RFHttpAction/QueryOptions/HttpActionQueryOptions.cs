using RFIServices.QueryOptions;

namespace RFHttpAction.QueryOptions;

public class HttpActionQueryOptions : CreatableEntityQueryOptions
{
    public string? Token { get; set; }
    public string? DataContains { get; set; }
    public bool IsNotClosed { get; set; }

    public HttpActionQueryOptions() { }

    public HttpActionQueryOptions(HttpActionQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Token = options.Token;
        DataContains = options.DataContains;
        IsNotClosed = options.IsNotClosed;
    }

    public override HttpActionQueryOptions Clone()
        => new(this);
}
