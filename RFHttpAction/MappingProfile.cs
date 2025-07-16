using AutoMapper;
using RFHttpAction.DTO;
using RFHttpAction.Entities;
using System.Text.Json;

namespace RFHttpAction
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<HttpActionType, HttpActionTypeDTO>();

            CreateMap<HttpAction, HttpActionResponse>()
                .ForMember(
                    dest => dest.Data,
                    opt => opt.MapFrom(src => DeserealizeJson(src.Data))
                );
        }

        public static object? DeserealizeJson(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
