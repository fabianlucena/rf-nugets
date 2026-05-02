namespace RFBaseEntities.QueryOptions
{
    public class UserQueryOptions : CommonEntityQueryOptions
    {
        public string? Username { get; set; }

        public UserQueryOptions() { }

        public UserQueryOptions(UserQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Username = options.Username;
        }
    }
}
