using System.ComponentModel.DataAnnotations;

namespace TicketSwaapAPI.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string TokenType { get; set; }
    }
}
