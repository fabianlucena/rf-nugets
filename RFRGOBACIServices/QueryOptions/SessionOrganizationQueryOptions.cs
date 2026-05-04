using RFBaseEntities.QueryOptions;

namespace RFRGOBACIServices.QueryOptions
{
    public class SessionDataQueryOptions : BaseQueryOptions
    {
        public SessionDataQueryOptions() { }

        public SessionDataQueryOptions(SessionDataQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override SessionDataQueryOptions Clone()
            => new(this);
    }
}
