using RFService.Repo;

namespace RFService.IRepo
{
    public interface IRepo<Entity>
    {
        Task<Entity> InsertAsync(Entity data);

        Task<Entity> GetSingleAsync(GetOptions options);

        Task<Entity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<Entity?> GetFirstOrDefaultAsync(GetOptions options);

        Task<IEnumerable<Entity>> GetListAsync(GetOptions options);

        Task<int> UpdateAsync(object data, GetOptions options);
    }
}
