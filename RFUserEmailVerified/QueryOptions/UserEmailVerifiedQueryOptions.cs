using RFIServices.QueryOptions;

namespace RFUserEmailVerified.QueryOptions
{
    public class UserEmailVerifiedQueryOptions : CommonEntityQueryOptions
    {
        public long? UserId { get; set; }
        public string? Email { get; set; }

        public UserEmailVerifiedQueryOptions() { }

        public UserEmailVerifiedQueryOptions(UserEmailVerifiedQueryOptions? options)
            : base(options)
        {
            if (options is null)
                return;

            UserId = options.UserId;
            Email = options.Email;
        }

        public override UserEmailVerifiedQueryOptions Clone()
            => new(this);
    }
}
