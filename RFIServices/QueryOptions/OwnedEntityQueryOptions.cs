using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class OwnedEntityQueryOptions : CommonEntityQueryOptions
{
    public bool? IsMine { get; set; } = null;

    public OwnedEntityQueryOptions() { }

    public OwnedEntityQueryOptions(OwnedEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IsMine = options.IsMine;
    }

    public override OwnedEntityQueryOptions Clone()
        => new(this);

    public override OwnedEntityQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        if (request.Query.TryGetValue("mine", out var value))
        {
            var stringValue = value.ToString().Trim();

            IsMine = stringValue == "1" || (bool.TryParse(stringValue, out var parsedBool) && parsedBool);
        }

        return this;
    }
}
