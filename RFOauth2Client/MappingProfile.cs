using AutoMapper;
using RFOauth2Client.DTO;

namespace RFOauth2Client
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuthorizeProvider, LoginProviderResponse>()
                .ForMember(
                    dest => dest.Label,
                    opt => opt.MapFrom(src => src.Label)
                )
                .ForMember(
                    dest => dest.URL,
                    opt => opt.MapFrom(src => src.Url +
                        $"?client_id={src.ClientId}" +
                        $"&redirect_uri={src.RedirectUri}" +
                        $"&response_type={(string.IsNullOrEmpty(src.ResponseType) ? "code" : src.ResponseType)}" +
                        (string.IsNullOrEmpty(src.Scope) ? "" : $"&scope={src.Scope}"))
                );
        }
    }
}
