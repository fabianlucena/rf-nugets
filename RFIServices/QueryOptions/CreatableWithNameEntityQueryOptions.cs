namespace RFIServices.QueryOptions;

public class CreatableWithNameEntityQueryOptions : CreatableEntityQueryOptions
{
    public string? Name { get; set; }
    public IEnumerable<string>? Names { get; set; }

    public CreatableWithNameEntityQueryOptions() { }

    public CreatableWithNameEntityQueryOptions(CreatableWithNameEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Name = options.Name;
        Names = options.Names;
    }

    public override CreatableWithNameEntityQueryOptions Clone()
        => new(this);
}
