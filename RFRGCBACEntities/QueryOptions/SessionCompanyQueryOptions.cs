using RFBaseEntities.QueryOptions;

namespace RFRGCBACEntities.QueryOptions
{
    public class SessionCompanyQueryOptions : CommonEntityQueryOptions
    {
        public bool IncludeSession { get; set; } = false;
        public bool IncludeCompany { get; set; } = false;
    }
}
