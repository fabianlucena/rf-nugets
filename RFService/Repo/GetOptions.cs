namespace RFService.Repo
{
    public class GetOptions
    {
        public int? Offset { get; set; }
        
        public int? Top { get; set; }

        public Dictionary<string, object?> Filters { get; set; } = [];
        
        public Dictionary<string, GetOptions> Include { get; set; } = [];

        public GetOptions() { }

        public GetOptions(GetOptions options)
        {
            Offset = options.Offset;
            Top = options.Top;
            Filters = new Dictionary<string, object?>(options.Filters);
            Include = new Dictionary<string, GetOptions>(options.Include);
        }
    }
}
