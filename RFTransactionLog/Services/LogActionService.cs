using RFService.Services;
using RFService.IRepo;
using RFTransactionLog.Entities;
using RFTransactionLog.IServices;

namespace RFTransactionLog.Services
{
    public class LogActionService(IRepo<Entities.LogAction> repo)
        : ServiceIdUuidName<IRepo<Entities.LogAction>, Entities.LogAction>(repo),
            ILogActionService
    {
    }
}
