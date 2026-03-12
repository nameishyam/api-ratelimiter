using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateLimiter.Infrastructure;

namespace RateLimiter.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("requests")]
    public async Task<IActionResult> TotalRequests()
    {
        var count = await context.ApiRequestLogs.CountAsync();
        return Ok(count);
    }

    [HttpGet("blocked")]
    public async Task<IActionResult> BlockedRequests()
    {
        var count = await context.ApiRequestLogs
            .Where(x => x.IsBlocked)
            .CountAsync();

        return Ok(count);
    }

    [HttpGet("per-client")]
    public async Task<IActionResult> RequestsPerClient()
    {
        var result = await context.ApiRequestLogs
            .GroupBy(x => x.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                TotalRequests = g.Count(),
                BlockedRequests = g.Count(x => x.IsBlocked)
            })
            .ToListAsync();

        return Ok(result);
    }
}