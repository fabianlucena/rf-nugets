using RFIServices.QueryOptions;

namespace RFLoggerProvider.QueryOptions
{
    public class LogQueryOptions : CommonEntityQueryOptions
    {
        public bool OrderByLogTimestampDesc { get; set; } = true;

        public LogQueryOptions() { }

        public LogQueryOptions(LogQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            OrderByLogTimestampDesc = options.OrderByLogTimestampDesc;
        }

        public override LogQueryOptions Clone()
            => new(this);
    }
}
