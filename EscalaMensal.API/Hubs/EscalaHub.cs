using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using EscalaMensal.Application.DTOs.Escala;
using Microsoft.AspNetCore.SignalR;

namespace EscalaMensal.API.Hubs
{
    public class EscalaHub : Hub
    {
        // Armazena as conexões ativas por sala: EscalaId -> (ConnectionId -> UsuarioConectadoDto)
        private static readonly ConcurrentDictionary<int, ConcurrentDictionary<string, UsuarioConectadoDto>> _salas = new();

        /// <summary>
        /// Usuário entra na sala da escala em tempo real
        /// </summary>
        public async Task EntrarNaEscala(int escalaId, string nomeUsuario, string emailUsuario, int usuarioId, string perfil, string permissao)
        {
            var sala = _salas.GetOrAdd(escalaId, _ => new ConcurrentDictionary<string, UsuarioConectadoDto>());

            var usuario = new UsuarioConectadoDto
            {
                ConnectionId = Context.ConnectionId,
                UsuarioId = usuarioId,
                Nome = string.IsNullOrWhiteSpace(nomeUsuario) ? "Usuário Anônimo" : nomeUsuario,
                Email = emailUsuario,
                Perfil = perfil,
                Permissao = permissao,
                HoraEntrada = DateTime.UtcNow
            };

            sala[Context.ConnectionId] = usuario;

            await Groups.AddToGroupAsync(Context.ConnectionId, $"Escala_{escalaId}");

            // Envia a lista atual de todos os usuários conectados para quem acabou de entrar
            await Clients.Caller.SendAsync("UsuariosConectados", sala.Values.ToList());

            // Notifica os outros membros da sala que um novo usuário entrou
            await Clients.OthersInGroup($"Escala_{escalaId}").SendAsync("UsuarioEntrou", usuario);
        }

        /// <summary>
        /// Usuário sai da sala voluntariamente (ex: ao navegar para outra página)
        /// </summary>
        public async Task SairDaEscala(int escalaId)
        {
            if (_salas.TryGetValue(escalaId, out var sala))
            {
                if (sala.TryRemove(Context.ConnectionId, out var usuario))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Escala_{escalaId}");
                    await Clients.Group($"Escala_{escalaId}").SendAsync("UsuarioSaiu", usuario.UsuarioId, usuario.Nome);
                }
            }
        }

        /// <summary>
        /// Notifica a alteração de um membro escalado em um item de missa
        /// </summary>
        public async Task NotificarMembroAlterado(int escalaId, int itemMissaId, int? membroId, string? membroNome, string alteradoPor)
        {
            await Clients.OthersInGroup($"Escala_{escalaId}").SendAsync("MembroAlterado", itemMissaId, membroId, membroNome, alteradoPor);
        }

        /// <summary>
        /// Notifica que a ordenação de itens foi alterada
        /// </summary>
        public async Task NotificarOrdemAlterada(int escalaId, int missaId, string alteradoPor)
        {
            await Clients.OthersInGroup($"Escala_{escalaId}").SendAsync("OrdemAlterada", missaId, alteradoPor);
        }

        /// <summary>
        /// Notifica que uma alteração estrutural ocorreu (nova missa, exclusão, etc)
        /// </summary>
        public async Task NotificarEscalaRecarregada(int escalaId, string mensagem, string alteradoPor)
        {
            await Clients.OthersInGroup($"Escala_{escalaId}").SendAsync("EscalaRecarregada", mensagem, alteradoPor);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var (escalaId, sala) in _salas)
            {
                if (sala.TryRemove(Context.ConnectionId, out var usuario))
                {
                    await Clients.Group($"Escala_{escalaId}").SendAsync("UsuarioSaiu", usuario.UsuarioId, usuario.Nome);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
