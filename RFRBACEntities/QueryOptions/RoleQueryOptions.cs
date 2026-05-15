using RFBaseEntities.QueryOptions;

namespace RFRBACEntities.QueryOptions
{
    public class RoleQueryOptions : CommonEntityQueryOptions
    {
        public IEnumerable<long>? Ids { get; set; }

        public RoleQueryOptions() { }

        public RoleQueryOptions(RoleQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Ids = options.Ids != null ? [.. options.Ids] : null;
        }

        public override RoleQueryOptions Clone()
            => new(this);
    }
}
