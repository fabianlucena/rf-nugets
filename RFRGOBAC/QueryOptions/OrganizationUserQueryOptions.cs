using Microsoft.AspNetCore.Http;
using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class OrganizationUserQueryOptions : UserQueryOptions
{
    public bool IncludeRoles { get; set; }
    public bool IncludeCanEdit { get; set; }

    public long? OrganizationId { get; set; }
    public long? UserId { get; set; }

    public OrganizationUserQueryOptions() { }

    public OrganizationUserQueryOptions(OrganizationUserQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeRoles = options.IncludeRoles;
        IncludeCanEdit = options.IncludeCanEdit;

        OrganizationId = options.OrganizationId;
        UserId = options.UserId;
    }

    public override OrganizationUserQueryOptions Clone()
        => new(this);

    public override OrganizationUserQueryOptions UpdateFromRequest)HttpRequest request)
    {
        base.UpdateFromRequest)request);

        if (request.Query.ContainsKey("includeRoles"))
            IncludeRoles = bool.Parse(request.Query["includeRoles"].ToString());

        if (request.Query.ContainsKey("includeCanEdit"))
            IncludeCanEdit = bool.Parse(request.Query["includeCanEdit"].ToString());

        return this;
    }
}
