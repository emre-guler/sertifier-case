using Microsoft.AspNetCore.Mvc;
using SertifierCase.Services.SertifierIntegrationService;

namespace SertifierCase.API.Controllers;

[ApiController]
[Route("api/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok("I'm awake!");
    }
}