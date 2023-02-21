using Confab.Modules.Agendas.Application.Submissions.Commands;
using Confab.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers;

internal class SubmissionsController : BaseController
{
    private readonly ICommandDispatcher _commandDispatcher;

    public SubmissionsController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateSubmission command)
    {
        await _commandDispatcher.SendAsync(command);
        return CreatedAtAction("Get", new { Id = command.Id }, null);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<ActionResult> ApproveAsync(Guid id)
    {
        await _commandDispatcher.SendAsync(new ApproveSubmission(id));
        return NoContent();
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<ActionResult> RejectAsync(Guid id)
    {
        await _commandDispatcher.SendAsync(new RejectSubmission(id));
        return NoContent();
    }
}
