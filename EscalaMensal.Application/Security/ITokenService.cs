using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Application.Security
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
