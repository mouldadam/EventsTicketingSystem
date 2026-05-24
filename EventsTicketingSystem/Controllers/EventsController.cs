using EventsTicketingSystem.Data;
using EventsTicketingSystem.Models;
using EventsTicketingSystem.Models.Dto;
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

    [HttpPost("{id:int}/reserve")]
    public async Task<ActionResult> ReserveTicket(int id, [FromBody] ReserveTicketRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.HolderName))
            {
                ModelState.AddModelError(nameof(request.HolderName), "The holderName field is required.");
                _logger.LogError("Ticket reservation rejected for event {EventId} because holder name was missing.", id);
                return ValidationProblem(ModelState);
            }

            var eventExists = await _context.Events.AnyAsync(e => e.Id == id);
            if (!eventExists)
            {
                _logger.LogError("Ticket reservation rejected because event {EventId} was not found.", id);
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Where(t => t.EventId == id && t.Status == TicketStatusEnum.TicketStatus.Available)
                .OrderBy(t => t.Id)
                .FirstOrDefaultAsync();

            if (ticket is null)
            {
                _logger.LogError("Ticket reservation failed for event {EventId} because no tickets were available.", id);
                return Conflict(new { message = "No available tickets for this event." });
            }

            ticket.HolderName = request.HolderName.Trim();
            ticket.Status = TicketStatusEnum.TicketStatus.Reserved;
            ticket.ReservedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Ticket {TicketId} reserved for holder {HolderName} on event {EventId}.", ticket.Id, ticket.HolderName, id);
            return Ok(new
            {
                ticket.Id,
                ticket.EventId,
                ticket.HolderName,
                ticket.Status,
                ticket.ReservedAt
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogError("Concurrency conflict while reserving a ticket for event {EventId}.", id);
            return Conflict(new
            {
                message = "Another customer reserved the last available ticket for this event."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while reserving a ticket for event {EventId}.", id);
            return StatusCode(500, "There was an error");
        }
    }
}