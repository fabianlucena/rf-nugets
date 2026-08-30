using Microsoft.AspNetCore.Http;

namespace RFIServices.QueryOptions
{
    public abstract class ALocalizableEntityQueryOptions : LocalizableEntityQueryOptions
    {
        public bool IncludeInactive { get; set; } = false;

        public ALocalizableEntityQueryOptions() { }

        public ALocalizableEntityQueryOptions(ALocalizableEntityQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;

            IncludeInactive = options.IncludeInactive;
        }

        public override CommonEntityQueryOptions BuildFromRequest(HttpRequest request)
        {
            base.BuildFromRequest(request);

            if (request.Query.TryGetValue("includeInactive", out var value))
            {
                var stringValue = value.ToString().Trim();

                IncludeInactive = stringValue == "1" || (bool.TryParse(stringValue, out var parsedBool) && parsedBool);
            }

            return this;
        }
    }
}
