using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RateLimiter.Infrastructure;

public class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=ep-morning-math-a8xdk7zt-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_aYp5cmVku8Wl;SSL Mode=Require;Trust Server Certificate=true"
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}