using RFIServices.QueryOptions;

namespace RFRGOBAC.QueryOptions;

public class ORGPDataQueryOptions : BaseQueryOptions
{
    public ORGPDataQueryOptions() { }

    public ORGPDataQueryOptions(ORGPDataQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override ORGPDataQueryOptions Clone()
        => new(this);
}
