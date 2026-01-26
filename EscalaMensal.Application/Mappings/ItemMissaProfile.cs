using AutoMapper;
using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Mappings
{
    public class ItemMissaProfile : Profile
    {
        public ItemMissaProfile()
        {
            CreateMap<ItemMissa, ItemMissaDto>();
        }
    }
}
