using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Speakers.Api.Controllers;

[ApiController]
[Route(SpeakersModule.BasePath + "/[controller]")]
public abstract class BaseController : Controller
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is not null)
        {
            return Ok(model);
        }

        return NotFound();
    }
}