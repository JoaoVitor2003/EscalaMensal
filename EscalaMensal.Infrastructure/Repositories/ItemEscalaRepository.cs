using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class EscalaItemRepository : IItemEscalaRepository
{
    private readonly AppDbContext _context;

    public EscalaItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ItemEscala>> ObterPorEscalaIdAsync(int escalaId)
    {
        return await _context.ItensEscala
            .Where(i => i.EscalaId == escalaId)
            .ToListAsync();
    }

    public async Task AdicionarAsync(ItemEscala item)
    {
        _context.ItensEscala.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(ItemEscala item)
    {
        _context.ItensEscala.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var item = await _context.ItensEscala.FindAsync(id);
        if (item != null)
        {
            _context.ItensEscala.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
