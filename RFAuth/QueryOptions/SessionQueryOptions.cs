using RFIServices.QueryOptions;

namespace RFAuth.QueryOptions
{
    public class SessionQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUser { get; set; }
        public bool IncludeDevice { get; set; }

        public string? AuthorizationToken { get; set; }
        public string? AutoLoginToken { get; set; }

        public SessionQueryOptions() { }

        public SessionQueryOptions(SessionQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeUser = options.IncludeUser;
            IncludeDevice = options.IncludeDevice;
            AuthorizationToken = options.AuthorizationToken;
            AutoLoginToken = options.AutoLoginToken;
        }

        public override SessionQueryOptions Clone()
            => new(this);
    }
}
