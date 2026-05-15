using RFBaseEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public sealed class SessionOrganizationQueryOptions : CommonEntityQueryOptions
    {
        public bool IncludeSession { get; set; } = false;
        public bool IncludeOrganization { get; set; } = false;

        public SessionOrganizationQueryOptions() { }

        public SessionOrganizationQueryOptions(SessionOrganizationQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeSession = options.IncludeSession;
            IncludeOrganization = options.IncludeOrganization;
        }

        public override SessionOrganizationQueryOptions Clone()
            => new(this);
    }
}
