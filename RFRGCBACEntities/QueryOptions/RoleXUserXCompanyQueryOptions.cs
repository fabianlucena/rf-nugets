using RFRBACEntities.QueryOptions;

namespace RFRGCBACEntities.QueryOptions
{
    public class RoleXUserXCompanyQueryOptions : RoleXUserQueryOptions
    {
        public bool IncludeCompany { get; set; } = false;
    }
}
