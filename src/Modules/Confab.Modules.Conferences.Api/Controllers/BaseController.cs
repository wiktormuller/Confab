using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers;

[Route(BasePath + "[controller]")]
[ApiController]
internal class BaseController : ControllerBase
{
    protected const string BasePath = "conferences-module";

    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }
}