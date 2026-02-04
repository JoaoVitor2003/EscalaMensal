using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class EscalaProfile : Profile
    {
        public EscalaProfile()
        {
            CreateMap<Escala, EscalaDto>();
            CreateMap<EscalaAdicionarDto, Escala>();
            CreateMap<EscalaAtualizarDto, Escala>();
            CreateMap<EscalaDeleteDto, Escala>();
        }
    }
}
