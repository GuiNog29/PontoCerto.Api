using AutoMapper;
using PontoCerto.Domain.Entities;
using PontoCerto.Application.DTOs;

namespace PontoCerto.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Pessoa, PessoaDto>().ReverseMap();
            CreateMap<RegistroPonto, RegistroPontoDto>().ReverseMap();
        }
    }
}
