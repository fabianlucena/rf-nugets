using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public sealed class SessionOrganizationQueryOptions : CommonEntityQueryOptions
{
    public bool IncludeSession { get; set; } = false;
    public bool IncludeOrganization { get; set; } = false;

    public long? SessionId { get; set; }
    public long? OrganizationId { get; set; }

    public SessionOrganizationQueryOptions() { }

    public SessionOrganizationQueryOptions(SessionOrganizationQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeSession = options.IncludeSession;
        IncludeOrganization = options.IncludeOrganization;

        SessionId = options.SessionId;
        OrganizationId = options.OrganizationId;
    }

    public override SessionOrganizationQueryOptions Clone()
        => new(this);
}
