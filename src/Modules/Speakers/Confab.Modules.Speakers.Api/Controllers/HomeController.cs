using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Speakers.Api.Controllers;

[Microsoft.AspNetCore.Components.Route(SpeakersModule.BasePath)]
public class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Speakers API";
}