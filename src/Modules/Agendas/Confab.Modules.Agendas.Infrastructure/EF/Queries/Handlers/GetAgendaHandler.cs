using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetAgendaHandler : IQueryHandler<GetAgenda, IEnumerable<AgendaTrackDto>>
{
    private readonly DbSet<AgendaTrack> _agendaTracks;

    public GetAgendaHandler(AgendasDbContext context)
    {
        _agendaTracks = context.AgendaTracks;
    }

    public async Task<IEnumerable<AgendaTrackDto>> HandleAsync(GetAgenda query)
    {
        var agendaTracks = await _agendaTracks
            .Include(at => at.Slots)
            .ThenInclude(at => (at as RegularAgendaSlot).AgendaItem)
            .ThenInclude(ai => ai.Speakers)
            .Where(at => at.ConferenceId == query.ConferenceId)
            .ToListAsync();

        var dtos = agendaTracks?.Select(at => at.AsDto());

        return dtos;
    }

    private static string GetStorageKey(Guid conferenceId) => $"agenda/{conferenceId}";
}
