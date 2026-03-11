using Microsoft.EntityFrameworkCore;
using RateLimiter.Domain.Entities;

namespace RateLimiter.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<RateLimit> RateLimits { get; set; }

    public DbSet<ApiRequestLog> ApiRequestLogs { get; set; }
}