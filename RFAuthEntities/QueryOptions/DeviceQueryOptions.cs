using RFBaseEntities.QueryOptions;

namespace RFAuthEntities.QueryOptions
{
    public class DeviceQueryOptions : CreatableEntityQueryOptions
    {
        public string? Token { get; set; }

        public DeviceQueryOptions() { }

        public DeviceQueryOptions(DeviceQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Token = options.Token;
        }

        public override DeviceQueryOptions Clone()
            => new(this);
    }
}
