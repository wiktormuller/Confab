using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;

namespace Confab.Modules.Attendances.Application.Clients.Agendas;

public interface IAgendasApiClient
{
    Task<RegularAgendaSlotDto> GetRegularAgendaSlotAsync(Guid id);
    Task<IEnumerable<AgendaTrackDto>> GetAgendaAsync(Guid conferenceId);
}