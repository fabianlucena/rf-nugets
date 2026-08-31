using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions
{
    public class RoleQueryOptions : LocalizableEntityQueryOptions
    {
        public bool IsSelectable { get; set; }

        public RoleQueryOptions() { }

        public RoleQueryOptions(RoleQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IsSelectable = options.IsSelectable;
        }

        public override RoleQueryOptions Clone()
            => new(this);
    }
}
