namespace RFBaseEntities.QueryOptions
{
    public abstract class JoinQueryOptions : BaseQueryOptions
    {
        public JoinQueryOptions() { }

        public JoinQueryOptions(JoinQueryOptions? options)
            : base(options)
        {
            if (options == null)
                return;
        }
    }
}
