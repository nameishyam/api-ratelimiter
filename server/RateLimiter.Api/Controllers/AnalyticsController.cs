using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateLimiter.Infrastructure;

namespace RateLimiter.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("requests")]
    public async Task<IActionResult> TotalRequests()
    {
        var count = await _context.ApiRequestLogs.CountAsync();
        return Ok(count);
    }

    [HttpGet("blocked")]
    public async Task<IActionResult> BlockedRequests()
    {
        var count = await _context.ApiRequestLogs
            .Where(x => x.IsBlocked)
            .CountAsync();

        return Ok(count);
    }

    [HttpGet("per-client")]
    public async Task<IActionResult> RequestsPerClient()
    {
        var result = await _context.ApiRequestLogs
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