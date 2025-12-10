using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class EscalaItemRepository : IItemMissaRepository
{
    private readonly AppDbContext _context;

    public EscalaItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ItemMissa>> ObterPorEscalaIdAsync(int escalaId)
    {
        return await _context.ItensMissa
            .Where(i => i.MissaId == escalaId)
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
}
