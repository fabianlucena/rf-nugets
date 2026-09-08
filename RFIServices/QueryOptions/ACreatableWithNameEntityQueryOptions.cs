namespace RFIServices.QueryOptions;

public class ACreatableWithNameEntityQueryOptions : CreatableWithNameEntityQueryOptions
{
    public bool IncludeInactive { get; set; } = false;

    public ACreatableWithNameEntityQueryOptions() { }

    public ACreatableWithNameEntityQueryOptions(ACreatableWithNameEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeInactive = options.IncludeInactive;
    }

    public override ACreatableWithNameEntityQueryOptions Clone()
        => new(this);
}
