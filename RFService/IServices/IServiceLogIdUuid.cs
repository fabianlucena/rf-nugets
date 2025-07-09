using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceLogIdUuid<TEntity>
        : IServiceLogId<TEntity>
        where TEntity : class
    {
    }
}
