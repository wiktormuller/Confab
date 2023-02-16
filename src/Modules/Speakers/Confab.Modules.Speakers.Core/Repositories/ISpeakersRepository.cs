using Confab.Modules.Speakers.Core.Entities;

namespace Confab.Modules.Speakers.Core.Repositories;

internal interface ISpeakersRepository
{
    Task<IReadOnlyList<Speaker>> BrowseAsync();
    Task<Speaker> GetAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(Speaker speaker);
    Task UpdateAsync(Speaker speaker);
}