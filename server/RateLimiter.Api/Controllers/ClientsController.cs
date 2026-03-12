using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateLimiter.Infrastructure;
using RateLimiter.Domain.Entities;
using RateLimiter.Api.DTOs;

namespace RateLimiter.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ApiKey = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        };

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        return Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        var clients = await context.Clients.ToListAsync();
        return Ok(clients);
    }
}