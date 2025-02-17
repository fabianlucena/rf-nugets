namespace RFService.IServices
{
    public interface IServiceUniqueTitle<TEntity>
        : IServiceTitle<TEntity>
        where TEntity : class
    {
    }
}
