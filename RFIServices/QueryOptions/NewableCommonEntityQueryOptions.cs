using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions;

public class NewableCommonEntityQueryOptions : CommonEntityQueryOptions
{
    public NewableCommonEntityQueryOptions() { }

    public NewableCommonEntityQueryOptions(CommonEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override NewableCommonEntityQueryOptions Clone()
        => new(this);
}
