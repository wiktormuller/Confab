using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Infrastructure.Clients.Requests;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Modules.Attendances.Infrastructure.Clients;

internal class AgendasApiClient : IAgendasApiClient
{
    private readonly IModuleClient _client;

    public AgendasApiClient(IModuleClient client)
    {
        _client = client;
    }

    public Task<IEnumerable<AgendaTrackDto>> GetAgendaAsync(Guid conferenceId)
    {
        return _client.SendAsync<IEnumerable<AgendaTrackDto>>("agendas/agendas/get",
        new GetAgenda
        {
            ConferenceId = conferenceId
        });
    }

    public Task<RegularAgendaSlotDto> GetRegularAgendaSlotAsync(Guid id)
    {
        return _client.SendAsync<RegularAgendaSlotDto>("agendas/slots/regular/get",
            new GetRegularAgendaSlot
            {
                AgendaItemId = id
            });
    }
}
