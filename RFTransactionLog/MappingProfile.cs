using AutoMapper;
using RFTransactionLog.Entities;
using RFTransactionLog.DTO;

namespace RFTransactionLog
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionLog, TransactionLogResponse>();
            CreateMap<LogLevel, LogLevelDTO>();
            CreateMap<LogAction, LogActionDTO>();
        }
    }
}
