using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Events;
using Confab.Modules.Speakers.Core.Exceptions;
using Confab.Modules.Speakers.Core.Mappings;
using Confab.Modules.Speakers.Core.Repositories;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Speakers.Core.Services;

internal class SpeakersService : ISpeakersService
{
    private readonly ISpeakersRepository _speakersRepository;
    private readonly IMessageBroker _messageBroker;

    public SpeakersService(ISpeakersRepository speakersRepository, 
        IMessageBroker messageBroker)
    {
        _speakersRepository = speakersRepository;
        _messageBroker = messageBroker;
    }

    public async Task<IEnumerable<SpeakerDto>> BrowserAsync()
    {
        var entities = await _speakersRepository.BrowseAsync();
        return entities?.Select(e => e.AsDto());
    }

    public async Task<SpeakerDto> GetASync(Guid speakerId)
    {
        var entity = await _speakersRepository.GetAsync(speakerId);
        return entity?.AsDto();
    }

    public async Task CreateAsync(SpeakerDto speakerDto)
    {
        var alreadyExists = await _speakersRepository.ExistsAsync(speakerDto.Id);
        if (alreadyExists)
        {
            throw new SpeakerAlreadyExistsException(speakerDto.Id);
        }

        await _speakersRepository.AddAsync(speakerDto.AsEntity());

        await _messageBroker.PublishAsync(new SpeakerCreated(speakerDto.Id, speakerDto.FullName));
    }

    public async Task UpdateAsync(SpeakerDto speaker)
    {
        var exists = await _speakersRepository.ExistsAsync(speaker.Id);

        if (!exists)
        {
            throw new SpeakerNotFoundException(speaker.Id);
        }

        await _speakersRepository.UpdateAsync(speaker.AsEntity());
    }
}