using RFRBACEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public class RoleXUserXCompanyQueryOptions : RoleXUserQueryOptions
    {
        public bool IncludeCompany { get; set; } = false;
    }
}
