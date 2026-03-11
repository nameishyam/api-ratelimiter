namespace RateLimiter.Domain.Entities;

public class RateLimit
{
    public int Id { get; set; }

    public Guid ClientId { get; set; }

    public int RequestsPerMinute { get; set; }

    public Client Client { get; set; }
}