using AutoMapper;
using EscalaMensal.Application.DTOs.HorarioFixo;
using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Application.Mappings
{
    public class HorarioFixoProfile : Profile
    {
        public HorarioFixoProfile()
        {
            CreateMap<HorarioFixo, HorarioFixoDto>();
            CreateMap<HorarioFixoAdicionarDto, HorarioFixo>();
        }
    }
}
