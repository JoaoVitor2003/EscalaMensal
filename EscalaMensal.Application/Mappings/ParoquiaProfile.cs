using AutoMapper;
using EscalaMensal.Application.DTOs.Paroquia;
using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Application.Mappings
{
    public class ParoquiaProfile : Profile
    {
        public ParoquiaProfile()
        {
            CreateMap<Paroquia, ParoquiaDto>()
                .ForMember(dest => dest.CriadoPorUsuarioNome, opt => opt.MapFrom(src => src.CriadoPorUsuario != null ? src.CriadoPorUsuario.Nome : null));

            CreateMap<Paroquia, ParoquiaResumoDto>()
                .ForMember(dest => dest.TotalMembros, opt => opt.MapFrom(src => src.Membros != null ? src.Membros.Count : 0));

            CreateMap<SolicitacaoAcessoParoquia, SolicitacaoAcessoParoquiaDto>()
                .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : string.Empty))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Email : string.Empty))
                .ForMember(dest => dest.ParoquiaNome, opt => opt.MapFrom(src => src.Paroquia != null ? src.Paroquia.Nome : string.Empty))
                .ForMember(dest => dest.RespondidoPorUsuarioNome, opt => opt.MapFrom(src => src.RespondidoPorUsuario != null ? src.RespondidoPorUsuario.Nome : null));

            CreateMap<ReivindicacaoParoquia, ReivindicacaoParoquiaDto>()
                .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : string.Empty))
                .ForMember(dest => dest.ParoquiaNome, opt => opt.MapFrom(src => src.Paroquia != null ? src.Paroquia.Nome : string.Empty));
        }
    }
}
