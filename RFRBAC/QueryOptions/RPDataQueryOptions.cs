using RFIServices.QueryOptions;

namespace RFRBAC.QueryOptions;

public class RPDataQueryOptions : BaseQueryOptions
{
    public RPDataQueryOptions() { }

    public RPDataQueryOptions(RPDataQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override RPDataQueryOptions Clone()
        => new(this);
}
