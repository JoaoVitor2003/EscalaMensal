using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

public class EscalaItemRepository : IItemMissaRepository
{
    private readonly AppDbContext _context;

    public EscalaItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ItemMissa?> ObterPorIdAsync(int id)
    {
        return await _context.ItensMissa
            .Include(i => i.Funcao)
            .Include(i => i.Usuario)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<ItemMissa?> ObterPorMissaIdAsync(int missaId)
    {
        return await _context.ItensMissa
            .Include(i => i.Funcao)
            .Include(i => i.Usuario)
            .FirstOrDefaultAsync(e => e.MissaId == missaId);
    }

    public async Task<int> QuantidadeDeEscalasDoUsuarioNaEscalaAsync(int escalaId, int usuarioId)
    {
        var missasIdsNaEscala = await _context.Missas
            .Where(m => m.EscalaId == escalaId)
            .Select(m => m.Id)
            .ToListAsync();

        return await _context.ItensMissa
            .CountAsync(i => i.UsuarioId == usuarioId && missasIdsNaEscala.Contains(i.MissaId));
    }

    public async Task AdicionarAsync(ItemMissa item)
    {
        await _context.ItensMissa.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(ItemMissa item)
    {
        _context.ItensMissa.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var item = await _context.ItensMissa.FindAsync(id);
        if (item != null)
        {
            _context.ItensMissa.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteUsuarioNaMissaAsync(int missaId, int usuarioId)
    {
        return await _context.ItensMissa.AnyAsync(m => m.MissaId == missaId && m.UsuarioId == usuarioId);
    }

    public async Task AtualizarOrdemItensMissa(
    List<ItemMissa> itens)
    {
        var ids = itens
            .Select(i => i.Id)
            .ToList();

        var entidades = await _context.ItensMissa
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        foreach (var entidade in entidades)
        {
            var itemAtualizado = itens
                .First(i => i.Id == entidade.Id);

            entidade.AtualizarOrdem(
                itemAtualizado.Ordem);
        }

        await _context.SaveChangesAsync();
    }

    public Task<bool> ExisteFuncaoNaMissaAsync(int missaId, int funcaoId)
    {
        return _context.ItensMissa.AnyAsync(m => m.MissaId == missaId && m.FuncaoId == funcaoId);
    }
}
