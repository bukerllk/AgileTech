using AgileTech_Web.Models.Dto;
using AutoMapper;

namespace AgileTech_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ClientDto, ClientCreateDto>().ReverseMap();
            CreateMap<ClientDto, ClientUpdateDto>().ReverseMap();
            CreateMap<ClientHubspotDto, ClientCreateHubspotDto>().ReverseMap();
        }
    }
}
