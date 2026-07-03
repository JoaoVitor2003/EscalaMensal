using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class ConfiguracaoRepository : IConfiguracaoRepository
    {
        private readonly AppDbContext _context;

        public ConfiguracaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> ObterMaxNivelAsync()
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync(c => c.Chave == "MaxNivel");
            if (config != null && int.TryParse(config.Valor, out var maxVal))
            {
                return maxVal;
            }
            return 3;
        }

        public async Task SalvarMaxNivelAsync(int maxNivel)
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync(c => c.Chave == "MaxNivel");
            if (config == null)
            {
                config = new Configuracao("MaxNivel", maxNivel.ToString());
                _context.Configuracoes.Add(config);
            }
            else
            {
                config.AtualizarValor(maxNivel.ToString());
                _context.Configuracoes.Update(config);
            }
            await _context.SaveChangesAsync();
        }
    }
}
