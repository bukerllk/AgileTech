using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using AutoMapper;

namespace AgileTech_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Client, ClientUpdateDto>().ReverseMap();
            CreateMap<Client, ClientCreateDto>();
            CreateMap<ClientCreateDto, Client>();
            
        }
    }
}
