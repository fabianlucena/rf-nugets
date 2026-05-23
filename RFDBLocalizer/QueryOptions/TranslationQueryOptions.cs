using RFIServices.QueryOptions;

namespace RFDBLocalizer.QueryOptions
{
    public class TranslationQueryOptions : CommonEntityQueryOptions
    {
        public string? Language { get; set; }
        public string? Context { get; set; }
        public string? Source { get; set; }

        public TranslationQueryOptions() { }

        public TranslationQueryOptions(TranslationQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            Language = options.Language;
            Context = options.Context;
            Source = options.Source;
        }

        public override TranslationQueryOptions Clone()
            => new(this);
    }
}
