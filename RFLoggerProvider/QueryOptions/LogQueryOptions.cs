using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFLoggerProvider.QueryOptions;

public class LogQueryOptions : CommonEntityQueryOptions
{
    public bool IncludeLevel { get; set; }
    public bool IncludeAction { get; set; }
    public bool IncludeSession { get; set; }
    public bool IncludeProject { get; set; }
    public bool IncludeUser { get; set; }

    public bool OrderByLogTimestampDesc { get; set; } = true;

    public LogQueryOptions() { }

    public LogQueryOptions(LogQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeLevel = options.IncludeLevel;
        IncludeAction = options.IncludeAction;
        IncludeSession = options.IncludeSession;
        IncludeProject = options.IncludeProject;
        IncludeUser = options.IncludeUser;

        OrderByLogTimestampDesc = options.OrderByLogTimestampDesc;
    }

    public override LogQueryOptions Clone()
        => new(this);

    public override LogQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        if (request.Query.TryGetValue("includeLevel", out var value))
        {
            var stringValue = value.ToString().Trim();
            IncludeLevel = stringValue == "1" || (bool.TryParse(stringValue, out var parsedBool) && parsedBool);
        }

        IncludeLevel = GetBoolFromRequest(request, "includeLevel");
        IncludeAction = GetBoolFromRequest(request, "includeAction");
        IncludeSession = GetBoolFromRequest(request, "includeSession");
        IncludeProject = GetBoolFromRequest(request, "includeProject");
        IncludeUser = GetBoolFromRequest(request, "includeUser");
        OrderByLogTimestampDesc = GetBoolFromRequest(request, "orderByLogTimestampDesc", OrderByLogTimestampDesc);

        return this;
    }
}
