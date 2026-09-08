namespace RFIServices.QueryOptions;

public class NominableOwnedEntityQueryOptions : OwnedEntityQueryOptions
{
    public string? Name { get; set; }
    public IEnumerable<string>? Names { get; set; }

    public NominableOwnedEntityQueryOptions() { }

    public NominableOwnedEntityQueryOptions(NominableOwnedEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        Name = options.Name;
        Names = options.Names;
    }

    public override NominableOwnedEntityQueryOptions Clone()
        => new(this);
}
