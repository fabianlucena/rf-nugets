using RFLoggerProvider.Entities;
using static RFDapper.Setup;

namespace RFLoggerProviderDapper
{
    public static class Setup
    {
        public static void ConfigureRFLoggerProviderDapper(IServiceProvider services)
        {
            CreateTable<LogLevel>(services);
            CreateTable<LogAction>(services);
            CreateTable<Log>(services);
        }
    }
}
