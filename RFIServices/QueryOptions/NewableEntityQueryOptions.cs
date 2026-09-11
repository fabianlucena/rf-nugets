namespace RFIServices.QueryOptions;

public class NewableEntityQueryOptions : EntityQueryOptions
{
    public NewableEntityQueryOptions() { }

    public NewableEntityQueryOptions(NewableEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;
    }

    public override NewableEntityQueryOptions Clone()
        => new NewableEntityQueryOptions(this);
}
