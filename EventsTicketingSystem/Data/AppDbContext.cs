using EventsTicketingSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace EventsTicketingSystem.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var seededEvent = new Event
            {
                Id = 1,
                Name = "Live Coding Lounge – Friday Night",
                StartsAt = new DateTime(2026, 1, 17, 19, 0, 0, DateTimeKind.Utc),
                TotalSeats = 50
            };

            var seededTickets = Enumerable.Range(1, 50)
                .Select(ticketNumber => new Ticket
                {
                    Id = ticketNumber,
                    EventId = seededEvent.Id,
                    HolderName = null,
                    Status = TicketStatusEnum.TicketStatus.Available,
                    ReservedAt = null
                })
                .ToArray();

            modelBuilder.Entity<Event>().HasData(seededEvent);
            modelBuilder.Entity<Ticket>().HasData(seededTickets);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .IsConcurrencyToken();
        }
    }
}
