using RFRBACEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public class RoleXUserXOrganizationQueryOptions : RoleXUserQueryOptions
    {
        public bool IncludeOrganization { get; set; } = false;
    }
}
