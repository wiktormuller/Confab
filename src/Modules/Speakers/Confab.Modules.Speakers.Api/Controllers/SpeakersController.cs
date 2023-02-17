using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Services;
using Confab.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Speakers.Api.Controllers;

[Authorize(Policy = POLICY)]
internal class SpeakersController : BaseController
{
    private const string POLICY = "speakers";
    private readonly ISpeakersService _speakersService;
    private readonly IContext _context;

    public SpeakersController(ISpeakersService speakersService, IContext context)
    {
        _speakersService = speakersService;
        _context = context;
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<SpeakerDto>> Get(Guid id) => OkOrNotFound(await _speakersService.GetASync(id));

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SpeakerDto>>> Get() => Ok(await _speakersService.BrowserAsync());

    [HttpPost]
    public async Task<ActionResult> Post(SpeakerDto speaker)
    {
        speaker.Id = _context.Identity.Id;
        await _speakersService.CreateAsync(speaker);
        return CreatedAtAction(nameof(Get), new { id = speaker.Id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(SpeakerDto speaker)
    {
        speaker.Id = _context.Identity.Id;
        await _speakersService.UpdateAsync(speaker);
        return NoContent();
    }
}