namespace RFIServices.QueryOptions
{
    public class UserTypeQueryOptions : CommonEntityQueryOptions
    {
        public UserTypeQueryOptions() { }

        public UserTypeQueryOptions(UserTypeQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }

        public override UserTypeQueryOptions Clone()
        {
            return new UserTypeQueryOptions(this);
        }
    }
}
