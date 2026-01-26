using AutoMapper;
using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class FuncaoProfile : Profile
    {
        public FuncaoProfile()
        {
            CreateMap<Funcao, FuncaoDto>();
        }
    }
}
