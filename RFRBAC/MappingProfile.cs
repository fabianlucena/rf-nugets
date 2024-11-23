using AutoMapper;
using RFRBAC.DTO;
using RFRBAC.Entities;

namespace RFRBAC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleResponse>();
            CreateMap<Role, RoleAttributes>();
            CreateMap<RoleAttributes, RoleResponse>();
        }
    }
}
