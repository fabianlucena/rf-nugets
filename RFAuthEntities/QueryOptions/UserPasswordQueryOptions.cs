using RFBaseEntities.QueryOptions;

namespace RFAuthEntities.QueryOptions
{
    public class UserPasswordQueryOptions : NoIdEntityQueryOptions
    {
        public bool IncludeUser { get; set; } = false;
    }
}
