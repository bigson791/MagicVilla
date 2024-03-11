using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API
{
    public class MappingConfig : Profile  // : Profile esta heredado del paquete automapper
    {
        public MappingConfig()
        {
            CreateMap<Villa, villaDto>();
            CreateMap<villaDto, Villa>();

            CreateMap<Villa, villaCreateDto>().ReverseMap(); // funciona tal y como el anterior
            CreateMap<Villa, villaUpdateDto>().ReverseMap(); // funciona tal y como el anterior

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}
