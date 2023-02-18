using Confab.Modules.Tickets.Core.DAL;
using Confab.Modules.Tickets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Tickets.Core.Repositories;

internal class TicketRepository : ITicketRepository
{
    private readonly TicketsDbContext _context;
    private readonly DbSet<Ticket> _tickets;
    
    public TicketRepository(TicketsDbContext context)
    {
        _context = context;
        _tickets = _context.Tickets;
    }

    public Task<Ticket> GetAsync(Guid conferenceId, Guid userId)
        => _tickets.SingleOrDefaultAsync(x => x.ConferenceId == conferenceId && x.UserId == userId);

    public Task<int> CountForConferenceAsync(Guid conferenceId)
        => _tickets.CountAsync(x => x.ConferenceId == conferenceId);

    public async Task<IReadOnlyList<Ticket>> GetForUserAsync(Guid userId)
        => await _tickets.Include(x => x.Conference)
            .Where(x => x.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(Ticket ticket)
    {
        await _tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task AddManyAsync(IEnumerable<Ticket> tickets)
    {
        await _tickets.AddRangeAsync(tickets);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ticket ticket)
    {
        _tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }
}