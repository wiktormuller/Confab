using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Agendas.Services;

public class AgendaTracksDomainService : IAgendaTracksDomainService
{
    private readonly IAgendaTracksRepository _agendaTracksRepository;
    private readonly IAgendaItemsRepository _agendaItemsRepository;

    public AgendaTracksDomainService(IAgendaTracksRepository agendaTracksRepository, 
        IAgendaItemsRepository agendaItemsRepository)
    {
        _agendaTracksRepository = agendaTracksRepository;
        _agendaItemsRepository = agendaItemsRepository;
    }

    // This domain service verify if some speaker doesn't have more than one agendaItem in the same time.
    // We cannot do this in aggregate, because aggregate cannot operate on more than one (itself) aggregate.
    public async Task AssignAgendaItemAsync(AgendaTrack agendaTrack, EntityId agendaSlotId, EntityId agendaItemId)
    {
        var agendaTracks = await _agendaTracksRepository.BrowseAsync(agendaTrack.ConferenceId);

        var slotToAssign = agendaTrack.Slots.OfType<RegularAgendaSlot>().SingleOrDefault(s => s.Id == agendaSlotId);

        if (slotToAssign is null)
        {
            throw new AgendaSlotNotFoundException(agendaSlotId);
        }

        var agendaitem = await _agendaItemsRepository.GetAsync((Guid)agendaItemId);

        if (agendaitem is null)
        {
            throw new AgendaItemNotFoundException((Guid)agendaItemId);
        }

        var speakerIds = agendaitem.Speakers.Select(s => new SpeakerId(s.Id));
        var speakersAgendaItems = await _agendaItemsRepository.BrowseAsync(speakerIds);
        var speakersAgendaItemsIds = speakersAgendaItems.Select(sai => (Guid)sai.Id).ToList();

        var hasCollidingSpeakerSlots = agendaTracks
            .SelectMany(at => at.Slots)
            .OfType<RegularAgendaSlot>()
            .Any(s => speakersAgendaItemsIds.Contains(s.Id) && s.From < slotToAssign.To && slotToAssign.From > s.To);

        if (hasCollidingSpeakerSlots)
        {
            throw new CollidingSpeakerAgendaSlotsException(agendaSlotId, agendaItemId);
        }

        agendaTrack.ChangeSlotAgendaItem(agendaSlotId, agendaitem);
    }
}
