using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public class RoleQueryOptions : LocalizableEntityQueryOptions
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
