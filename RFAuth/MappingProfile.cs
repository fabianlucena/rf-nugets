using AutoMapper;
using RFAuth.DTO;
using RFAuth.Entities;

namespace RFAuth
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<LoginData, LoginResponse>();
        }
    }
}
