using RFIRepositories.IRepositories;
using RFLoggerProvider.Entities;

namespace RFLoggerProvider.IRepositories
{
    public interface ILogRepository : ICommonEntityRepository<Log>
    {
    }
}