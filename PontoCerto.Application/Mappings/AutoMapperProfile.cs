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
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<RegistroPonto, RegistroPontoDto>()
                .ForMember(x => x.Data, opt => opt.MapFrom(src => src.Data.ToString("dd/MM/yyyy")))
                .ForMember(x => x.HoraEntrada, opt => opt.MapFrom(src => src.HoraEntrada.ToString(@"hh\:mm")))
                .ForMember(x => x.InicioAlmoco, opt => opt.MapFrom(src => src.InicioAlmoco.ToString(@"hh\:mm")))
                .ForMember(x => x.FimAlmoco, opt => opt.MapFrom(src => src.FimAlmoco.ToString(@"hh\:mm")))
                .ForMember(x => x.HoraSaida, opt => opt.MapFrom(src => src.HoraSaida.ToString(@"hh\:mm")))
                .ReverseMap();
        }
    }
}
