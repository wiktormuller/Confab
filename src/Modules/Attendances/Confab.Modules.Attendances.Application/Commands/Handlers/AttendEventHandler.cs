using Confab.Modules.Attendances.Application.Exceptions;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Attendances.Application.Commands.Handlers;

internal sealed class AttendEventHandler : ICommandHandler<AttendEvent>
{
    private readonly IAttendableEventsRepository _attendableEventsRepository;
    private readonly IParticipantsRepository _participantsRepository;

    public AttendEventHandler(IAttendableEventsRepository attendableEventsRepository, 
        IParticipantsRepository participantsRepository)
    {
        _attendableEventsRepository = attendableEventsRepository;
        _participantsRepository = participantsRepository;
    }

    public async Task HandleAsync(AttendEvent command)
    {
        var attendableEvent = await _attendableEventsRepository.GetAsync(command.Id);
        if (attendableEvent is null)
        {
            throw new AttendableEventNotFoundException(command.Id);
        }

        var participant = await _participantsRepository
            .GetAsync(attendableEvent.ConferenceId, command.ParticipantId);

        if (participant is null)
        {
            throw new ParticipantNotFoundException(attendableEvent.ConferenceId, command.ParticipantId);
        }

        attendableEvent.Attend(participant);

        // We are updating here two aggregates at once in one transaction (what is not correct with pure DDD)
        await _participantsRepository.UpdateAsync(participant);
        await _attendableEventsRepository.UpdateAsync(attendableEvent);
    }
}
