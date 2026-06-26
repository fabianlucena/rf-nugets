using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public class RoleQueryOptions : LocalizableEntityQueryOptions
    {
        public RoleQueryOptions() { }

        public RoleQueryOptions(RoleQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override RoleQueryOptions Clone()
            => new(this);
    }
}
