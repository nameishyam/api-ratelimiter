using Microsoft.AspNetCore.Mvc;

namespace RateLimiter.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API request successful");
    }
}