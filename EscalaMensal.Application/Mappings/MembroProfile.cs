using AutoMapper;
using EscalaMensal.Application.DTOs.Membro;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class MembroProfile : Profile
    {
        public MembroProfile()
        {
            CreateMap<Membro, MembroDto>();
            CreateMap<MembroAdicionarDto, Membro>();
            CreateMap<MembroAtualizarDto, Membro>();
        }
    }
}
