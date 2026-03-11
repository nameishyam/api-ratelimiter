namespace RateLimiter.Api.DTOs;

public class SetRateLimitRequest
{
    public Guid ClientId { get; set; }
    public int RequestsPerMinute { get; set; }
}