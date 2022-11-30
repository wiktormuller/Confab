using Confab.Modules.Conferences.Core.Entities;

namespace Confab.Modules.Conferences.Core.Repositories;

internal interface IHostRepository
{
    Task<Host?> GetAsync(Guid id);
    Task<IReadOnlyList<Host>> BrowserAsync();
    Task AddAsync(Host host);
    Task UpdateAsync(Host host);
    Task DeleteAsync(Host host);
}