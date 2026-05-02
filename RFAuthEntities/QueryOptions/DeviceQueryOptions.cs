using RFBaseEntities.QueryOptions;

namespace RFAuthEntities.QueryOptions
{
    public class DeviceQueryOptions : CreatableEntityQueryOptions
    {
        public string? Token { get; set; }

        public DeviceQueryOptions() { }

        public DeviceQueryOptions(DeviceQueryOptions options)
        {
            Token = options.Token;
        }
    }
}
