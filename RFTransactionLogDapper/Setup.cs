using RFTransactionLog.Entities;
using static RFDapper.Setup;

namespace RFTransactionLogDapper
{
    public static class Setup
    {
        public static void ConfigureRFTransactionLogDapper(IServiceProvider services)
        {
            CreateTable<LogLevel>(services);
            CreateTable<LogAction>(services);
            CreateTable<TransactionLog>(services);
        }
    }
}
