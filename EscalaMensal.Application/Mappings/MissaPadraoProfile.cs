using AutoMapper;
using EscalaMensal.Application.DTOs.MissaPadrao;
using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Application.Mappings
{
    public class MissaPadraoProfile : Profile
    {
        public MissaPadraoProfile()
        {
            CreateMap<MissaPadrao, MissaPadraoDto>();
            CreateMap<MissaPadraoAdicionarDto, MissaPadrao>()
                .ForMember(dest => dest.Funcoes, opt => opt.Ignore());
        }
    }
}
