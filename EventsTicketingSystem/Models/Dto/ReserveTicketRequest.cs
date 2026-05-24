using System.ComponentModel.DataAnnotations;

namespace EventsTicketingSystem.Models.Dto
{
    public class ReserveTicketRequest
    {
        [Required]
        public string HolderName { get; set; } = string.Empty;
    }
}
