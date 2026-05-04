using RFBaseEntities.QueryOptions;

namespace RFPermissionsEntities.QueryOptions
{
    public class PermissionQueryOptions : ImmutableEntityQueryOptions
    {
        public IEnumerable<long>? Ids { get; set; }

        public PermissionQueryOptions() { }

        public PermissionQueryOptions(PermissionQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Ids = options.Ids;
        }

        public override PermissionQueryOptions Clone()
            => new(this);
    }
}
