using AutoMapper;
using EscalaMensal.Application.DTOs.HistoricoEscala;
using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Application.Mappings
{
    public class HistoricoEscalaProfile : Profile
    {
        public HistoricoEscalaProfile()
        {
            CreateMap<HistoricoEscala, HistoricoEscalaDto>();
            CreateMap<HistoricoMissa, HistoricoMissaDto>();
            CreateMap<HistoricoItemMissa, HistoricoItemMissaDto>();
        }
    }
}
