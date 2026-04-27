using RFBaseEntities.QueryOptions;

namespace RFAuthEntities.QueryOptions
{
    public class SessionQueryOptions : CreatableEntityQueryOptions
    {
        public bool IncludeUser { get; set; } = false;
        public bool IncludeDevice { get; set; } = false;
    }
}
