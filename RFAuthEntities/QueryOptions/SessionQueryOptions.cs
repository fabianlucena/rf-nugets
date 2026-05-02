using RFBaseEntities.QueryOptions;

namespace RFAuthEntities.QueryOptions
{
    public class SessionQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUser { get; set; }
        public bool IncludeDevice { get; set; }

        public string? Token { get; set; }
        public string? AutoLoginToken { get; set; }


        public SessionQueryOptions() { }

        public SessionQueryOptions(SessionQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeUser = options.IncludeUser;
            IncludeDevice = options.IncludeDevice;
            Token = options.Token;
            AutoLoginToken = options.AutoLoginToken;
        }
    }
}
