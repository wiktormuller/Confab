using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Exceptions;
using Confab.Modules.Speakers.Core.Mappings;
using Confab.Modules.Speakers.Core.Repositories;

namespace Confab.Modules.Speakers.Core.Services;

internal class SpeakersService : ISpeakersService
{
    private readonly ISpeakersRepository _speakersRepository;
    
    public SpeakersService(ISpeakersRepository speakersRepository)
    {
        _speakersRepository = speakersRepository;
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