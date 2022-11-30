using Confab.Modules.Conferences.Core.Entities;

namespace Confab.Modules.Conferences.Core.Repositories;

internal class InMemoryHostRepository : IHostRepository
{
    // Not thread-safe, use Concurrent collection
    private readonly List<Host> _hosts = new();

    public Task<Host?> GetAsync(Guid id) => Task.FromResult(_hosts.SingleOrDefault(x => x.Id == id));

    public async Task<IReadOnlyList<Host>> BrowserAsync()
    {
        await Task.CompletedTask;
        return _hosts;
    }

    public Task AddAsync(Host host)
    {
        _hosts.Add(host);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Host host)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Host host)
    {
        _hosts.Remove(host);
        return Task.CompletedTask;
    }
}