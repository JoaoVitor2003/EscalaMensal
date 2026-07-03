using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IConfiguracaoRepository
    {
        Task<int> ObterMaxNivelAsync();
        Task SalvarMaxNivelAsync(int maxNivel);
    }
}
