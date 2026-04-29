using RFBaseEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public class SessionOrganizationQueryOptions : CommonEntityQueryOptions
    {
        public bool IncludeSession { get; set; } = false;
        public bool IncludeOrganization { get; set; } = false;
    }
}
