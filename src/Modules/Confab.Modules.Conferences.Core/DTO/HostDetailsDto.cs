namespace Confab.Modules.Conferences.Core.DTO;

internal class HostDetailsDto : HostDto
{
    public List<ConferenceDto> Conferences { get; set; }
}