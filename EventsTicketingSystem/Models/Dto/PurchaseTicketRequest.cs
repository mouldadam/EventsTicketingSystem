using System.ComponentModel.DataAnnotations;

namespace EventsTicketingSystem.Models.Dto
{
    public class PurchaseTicketRequest
    {
        [Required]
        public string HolderName { get; set; } = string.Empty;
    }
}
