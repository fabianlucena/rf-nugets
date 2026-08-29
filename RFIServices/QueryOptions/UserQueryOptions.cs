using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class UserQueryOptions : CommonEntityQueryOptions
{
    public string? Username { get; set; }

    public Guid? TypeUuid { get; set; }

    public UserQueryOptions() { }

    public UserQueryOptions(UserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Username = options.Username;
        TypeUuid = options.TypeUuid;
    }

    public override UserQueryOptions Clone()
        => new(this);

    public UserQueryOptions BuildFromRequest(HttpRequest request, UserQueryOptions? options = null)
    {
        options ??= new UserQueryOptions();

        base.BuildFromRequest(request, options);

        if (request.Query.ContainsKey("username"))
            options.Username = request.Query["username"].ToString();

        if (request.Query.ContainsKey("typeUuid"))
            options.TypeUuid = Guid.Parse(request.Query["typeUuid"].ToString());

        return options;
    }
}
