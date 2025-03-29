using AutoMapper;
using RFLoggerProvider.DTO;
using RFLoggerProvider.Entities;

namespace RFLoggerProvider
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<Log, LogResponse>();
            CreateMap<LogLevel, LogLevelDTO>();
            CreateMap<LogAction, LogActionDTO>();
        }
    }
}
