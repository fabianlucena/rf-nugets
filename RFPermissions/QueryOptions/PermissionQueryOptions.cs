using RFIServices.QueryOptions;

namespace RFPermissions.QueryOptions
{
    public class PermissionQueryOptions : ImmutableEntityQueryOptions
    {
        public IEnumerable<string>? Names { get; set; }

        public PermissionQueryOptions() { }

        public PermissionQueryOptions(PermissionQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Names = options.Names;
        }

        public override PermissionQueryOptions Clone()
            => new(this);
    }
}
