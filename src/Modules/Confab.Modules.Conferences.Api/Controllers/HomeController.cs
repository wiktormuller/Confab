using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers;

internal class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Conferences API";
}