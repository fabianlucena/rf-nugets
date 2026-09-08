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

        if (request.Query.TryGetValue("includeInactive", out var value))
        {
            var stringValue = value.ToString().Trim();

            IncludeInactive = stringValue == "1" || (bool.TryParse(stringValue, out var parsedBool) && parsedBool);
        }

        return this;
    }
}
