using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class UserQueryOptions : CommonEntityQueryOptions
{
    public string? Username { get; set; }

    public Guid? TypeUuid { get; set; }

    public bool IncludeType { get; set; }

    public UserQueryOptions() { }

    public UserQueryOptions(UserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Username = options.Username;
        TypeUuid = options.TypeUuid;

        IncludeType = options.IncludeType;
    }

    public override UserQueryOptions Clone()
        => new(this);

    public override UserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("username"))
            Username = request.Query["username"].ToString();

        if (request.Query.ContainsKey("typeUuid"))
            TypeUuid = Guid.Parse(request.Query["typeUuid"].ToString());

        if (request.Query.ContainsKey("includeType"))
            IncludeType = bool.Parse(request.Query["includeType"].ToString());

        return this;
    }
}
