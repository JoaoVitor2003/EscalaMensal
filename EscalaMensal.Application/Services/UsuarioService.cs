using AutoMapper;
using EscalaMensal.Application.DTOs.Usuario;     
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services      
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            var dto = _mapper.Map<UsuarioDto>(usuario);

            return dto;
        }

        public async Task<List<UsuarioDto>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);
            return usuariosDto;
        }

        public async Task AdicionarAsync(UsuarioAdicionarDto usuario)
        {
            var usuarioEntity = _mapper.Map<Usuario>(usuario);

            await _usuarioRepository.AdicionarAsync(usuarioEntity);
        }

        public async Task AtualizarAsync(UsuarioAtualizarDto usuarioDto)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioDto.Id);

            if (usuario == null)
                throw new DomainException("Usuário não encontrado.");

            _mapper.Map(usuarioDto, usuario);
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task RemoverAsync(int id)
        {
            await _usuarioRepository.RemoverAsync(id);
        }
    }
}
