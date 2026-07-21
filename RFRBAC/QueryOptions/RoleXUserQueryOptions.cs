namespace RFRBAC.QueryOptions
{
    public sealed class RoleXUserQueryOptions : RoleXUserQueryOptionsBase
    {
        public RoleXUserQueryOptions() { }

        public RoleXUserQueryOptions(RoleXUserQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override RoleXUserQueryOptions Clone()
            => new(this);
    }
}
