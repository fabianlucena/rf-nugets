using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class OrganizationUserQueryOptions : UserQueryOptions
{
    public bool IncludeRoles { get; set; }

    public long? OrganizationId { get; set; }
    public long? UserId { get; set; }

    public OrganizationUserQueryOptions() { }

    public OrganizationUserQueryOptions(OrganizationUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeRoles = options.IncludeRoles;

        OrganizationId = options.OrganizationId;
        UserId = options.UserId;
    }

    public override OrganizationUserQueryOptions Clone()
        => new(this);

    public override OrganizationUserQueryOptions BuildFromRequest(HttpRequest request)
    {
        base.BuildFromRequest(request);

        if (request.Query.ContainsKey("includeRoles"))
            IncludeRoles = bool.Parse(request.Query["includeRoles"].ToString());

        return this;
    }
}
