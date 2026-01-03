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

    public async Task<List<ItemMissa>> ObterPorMissaIdAsync(int missaId)
    {
        return await _context.ItensMissa
            .Include(i => i.Funcao)
            .Include(i => i.Usuario)
            .Where(i => i.MissaId == missaId)
            .ToListAsync();
    }

    public async Task AdicionarAsync(ItemMissa item)
    {
        _context.ItensMissa.Add(item);
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
}
