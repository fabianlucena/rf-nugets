namespace RFBaseEntities.QueryOptions
{
    public class UserQueryOptions : CommonEntityQueryOptions
    {
        public IEnumerable<long>? Ids { get; set; }

        public string? Username { get; set; }

        public UserQueryOptions() { }

        public UserQueryOptions(UserQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Ids = options.Ids != null ? [.. options.Ids] : null;
            Username = options.Username;
        }

        public override UserQueryOptions Clone()
        {
            return new UserQueryOptions(this);
        }
    }
}
