using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public abstract class CommonEntityQueryOptions : AuditableEntityQueryOptions
{
    public bool IncludeDeleted { get; set; } = false;
    public bool IncludeDeletedBy { get; set; } = false;

    public CommonEntityQueryOptions() { }

    public CommonEntityQueryOptions(CommonEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeDeleted = options.IncludeDeleted;
        IncludeDeletedBy = options.IncludeDeletedBy;
    }

    public override CommonEntityQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.TryGetValue("includeDeleted", out var value))
        {
            var stringValue = value.ToString().Trim();

            IncludeDeleted = stringValue == "1" || (bool.TryParse(stringValue, out var parsedBool) && parsedBool);
        }

        return this;
    }
}
