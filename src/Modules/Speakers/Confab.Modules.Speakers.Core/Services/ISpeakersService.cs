using Confab.Modules.Speakers.Core.DTO;

namespace Confab.Modules.Speakers.Core.Services;

internal interface ISpeakersService
{
    Task<IEnumerable<SpeakerDto>> BrowserAsync();
    Task<SpeakerDto> GetASync(Guid speakerId);
    Task CreateAsync(SpeakerDto speakerDto);
    Task UpdateAsync(SpeakerDto speaker);
}