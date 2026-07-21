using RFAuth.Entities;
using RFIRepositories.IRepositories;

namespace RFAuth.IRepositories
{
    public interface ISessionRepository : ICreatableEntityRepository<Session>
    {
    }
}