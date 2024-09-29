namespace TicketSwaapAPI.Models
{
    public class LogonModel
    {
        public string AccessToken { get; set; }
        public long AccessTokenExpirationTime { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}


