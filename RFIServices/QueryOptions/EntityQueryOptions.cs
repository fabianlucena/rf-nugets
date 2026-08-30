using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public abstract class EntityQueryOptions : BaseQueryOptions
{
    public long? Id { get; set; }
    public IEnumerable<long>? Ids { get; set; }
    public Guid? Uuid { get; set; }
    public IEnumerable<Guid>? Uuids { get; set; }

    public bool SkipOrderById { get; set; }

    public EntityQueryOptions() { }

    public EntityQueryOptions(EntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Id = options.Id;
        Ids = options.Ids != null ? [.. options.Ids] : null;
        Uuid = options.Uuid;
        Uuids = options.Uuids != null ? [.. options.Uuids] : null;

        SkipOrderById = options.SkipOrderById;
    }

    public override EntityQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("uuids"))
            Uuids = request.Query["uuids"].ToString().Split(',').Select(Guid.Parse);

        return this;
    }
}
