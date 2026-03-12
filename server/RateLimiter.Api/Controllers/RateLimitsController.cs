using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateLimiter.Api.DTOs;
using RateLimiter.Domain.Entities;
using RateLimiter.Infrastructure;

namespace RateLimiter.Api.Controllers;

[ApiController]
[Route("api/ratelimits")]
public class RateLimitsController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SetRateLimit([FromBody] SetRateLimitRequest request)
    {
        var existing = await context.RateLimits
            .FirstOrDefaultAsync(x => x.ClientId == request.ClientId);

        if (existing != null)
        {
            existing.RequestsPerMinute = request.RequestsPerMinute;
        }
        else
        {
            var rateLimit = new RateLimit
            {
                ClientId = request.ClientId,
                RequestsPerMinute = request.RequestsPerMinute
            };

            context.RateLimits.Add(rateLimit);
        }

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetRateLimits()
    {
        var limits = await context.RateLimits
            .Include(r => r.Client)
            .ToListAsync();

        return Ok(limits);
    }
}