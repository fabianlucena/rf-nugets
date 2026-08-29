namespace RFIServices.QueryOptions;

public abstract class CreatableEntityQueryOptions : EntityQueryOptions
{
    public bool IncludeCreatedBy { get; set; } = false;

    public DateTime? CreatedAfter { get; set; }

    public CreatableEntityQueryOptions() { }

    public CreatableEntityQueryOptions(CreatableEntityQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeCreatedBy = options.IncludeCreatedBy;

        CreatedAfter = options.CreatedAfter;
    }
}
