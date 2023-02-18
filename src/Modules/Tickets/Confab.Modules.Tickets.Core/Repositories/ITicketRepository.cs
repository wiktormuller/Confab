using Confab.Modules.Tickets.Core.Entities;

namespace Confab.Modules.Tickets.Core.Repositories;

internal interface ITicketRepository
{
    Task<Ticket> GetAsync(Guid conferenceId, Guid userId);
    Task<int> CountForConferenceAsync(Guid conferenceId);
    Task<IReadOnlyList<Ticket>> GetForUserAsync(Guid userId);
    Task AddAsync(Ticket ticket);
    Task AddManyAsync(IEnumerable<Ticket> tickets);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(Ticket ticket);
}