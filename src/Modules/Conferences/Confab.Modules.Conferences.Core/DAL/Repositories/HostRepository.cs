using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Conferences.Core.DAL.Repositories;

internal class HostRepository : IHostRepository
{
    private readonly ConferencesDbContext _context;
    private readonly DbSet<Host> _hosts;

    public HostRepository(ConferencesDbContext context)
    {
        _context = context;
        _hosts = _context.Hosts;
    }

    public Task<Host?> GetAsync(Guid id)
        => _hosts
        .Include(x => x.Conferences)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Host>> BrowserAsync()
        => await _hosts.ToListAsync();

    public async Task AddAsync(Host host)
    {
        await _hosts.AddAsync(host);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Host host)
    {
        _hosts.Update(host);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Host host)
    {
        _hosts.Remove(host);
        await _context.SaveChangesAsync();
    }
}