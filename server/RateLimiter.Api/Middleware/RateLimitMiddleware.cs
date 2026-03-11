using Microsoft.EntityFrameworkCore;
using RateLimiter.Domain.Entities;
using RateLimiter.Infrastructure;

namespace RateLimiter.Api.Middleware;

public class RateLimitMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ApplicationDbContext db)
    {
        var path = context.Request.Path.Value?.ToLower();

        if (path.StartsWith("/swagger") ||
            path.StartsWith("/api/clients") ||
            path.StartsWith("/api/ratelimits"))
        {
            await next(context);
            return;
        }

        var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);

        if (client == null)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        var rateLimit = await db.RateLimits
            .FirstOrDefaultAsync(r => r.ClientId == client.Id);

        if (rateLimit == null)
        {
            await next(context);
            return;
        }

        var windowStart = DateTime.UtcNow.AddMinutes(-1);

        var requestCount = await db.ApiRequestLogs
            .Where(r => r.ClientId == client.Id && r.Timestamp > windowStart)
            .CountAsync();

        if (requestCount >= rateLimit.RequestsPerMinute)
        {
            db.ApiRequestLogs.Add(new ApiRequestLog
            {
                ClientId = client.Id,
                Endpoint = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                IsBlocked = true
            });

            await db.SaveChangesAsync();

            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Rate limit exceeded");
            return;
        }

        db.ApiRequestLogs.Add(new ApiRequestLog
        {
            ClientId = client.Id,
            Endpoint = context.Request.Path,
            Timestamp = DateTime.UtcNow,
            IsBlocked = false
        });

        await db.SaveChangesAsync();

        await next(context);
    }
}