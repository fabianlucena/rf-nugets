using RFBaseEntities.QueryOptions;

namespace RFRGOBACEntities.QueryOptions
{
    public class OrganizationQueryOptions : CommonEntityQueryOptions
    {
        public OrganizationQueryOptions() { }

        public OrganizationQueryOptions(OrganizationQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override OrganizationQueryOptions Clone()
            => new(this);
    }
}
