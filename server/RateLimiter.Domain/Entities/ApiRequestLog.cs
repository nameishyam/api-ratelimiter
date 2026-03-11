namespace RateLimiter.Domain.Entities;

public class ApiRequestLog
{
    public int Id { get; set; }

    public Guid ClientId { get; set; }

    public string Endpoint { get; set; }

    public DateTime Timestamp { get; set; }

    public bool IsBlocked { get; set; }

    public Client Client { get; set; }
}