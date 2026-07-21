using RFIServices.QueryOptions;

namespace RFAuth.QueryOptions
{
    public class UserPasswordQueryOptions : NoIdEntityQueryOptions
    {
        public bool IncludeUser { get; set; }

        public long? UserId { get; set; }

        public UserPasswordQueryOptions() { }

        public UserPasswordQueryOptions(UserPasswordQueryOptions options)
            : base(options)
        {
            IncludeUser = options.IncludeUser;
            UserId = options.UserId;
        }

        public override UserPasswordQueryOptions Clone()
            => new(this);
    }
}
