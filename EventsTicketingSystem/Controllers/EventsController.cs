using EventsTicketingSystem.Data;
using EventsTicketingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<EventsController> _logger;

    public EventsController(AppDbContext context, ILogger<EventsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetEventDetails(int id)
    {
        var eventDetails = await _context.Events
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Name,
                e.StartsAt,
                e.TotalSeats,
                AvailableTickets = e.Tickets.Count(t => t.Status == TicketStatusEnum.TicketStatus.Available),
                ReservedTickets = e.Tickets.Count(t => t.Status == TicketStatusEnum.TicketStatus.Reserved),
                SoldTickets = e.Tickets.Count(t => t.Status == TicketStatusEnum.TicketStatus.Sold)
            })
            .FirstOrDefaultAsync();

        if (eventDetails is null)
        {
            _logger.LogWarning("Event details requested for non-existent event {EventId}.", id);
            return NotFound();
        }

        _logger.LogInformation("Event details returned for event {EventId}.", id);
        return Ok(eventDetails);
    }
}