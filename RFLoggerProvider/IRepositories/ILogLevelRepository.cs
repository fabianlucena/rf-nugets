using RFIRepositories.IRepositories;
using RFLoggerProvider.Entities;

namespace RFLoggerProvider.IRepositories
{
    public interface ILogLevelRepository : INominableEntityRepository<LogLevel>
    {
    }
}