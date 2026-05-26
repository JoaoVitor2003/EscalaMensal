using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class RestricaoProfile : Profile
    {
        public RestricaoProfile()
        {
            CreateMap<Restricao, RestricaoDto>();
            CreateMap<RestricaoAdicionarDto, Restricao>();
            CreateMap<RestricaoAtualizarDto, Restricao>();
            CreateMap<EscalaDeleteDto, Escala>();
        }
    }
}
