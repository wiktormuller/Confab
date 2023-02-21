using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Agendas.Application.Submissions.Events.External;

public sealed class SpeakerCreatedHandler : IEventHandler<SpeakerCreated>
{
    private readonly ISpeakerRepository _speakerRepository;

    public SpeakerCreatedHandler(ISpeakerRepository speakerRepository)
    {
        _speakerRepository = speakerRepository;
    }

    public async Task HandleAsync(SpeakerCreated @event)
    {
        if (await _speakerRepository.ExistsAsync(@event.Id)) // Via this line the action is idempotent
        {
            return;
        }

        var speaker = new Speaker(@event.Id, @event.FullName);
        await _speakerRepository.AddAsync(speaker);
    }
}
