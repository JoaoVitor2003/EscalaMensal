using AutoMapper;
using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class MissaProfile : Profile
    {
        public MissaProfile()
        {
            CreateMap<Missas, MissasDto>();
            CreateMap<MissaAdicionarDto, Missas>();
        }
    }
}
