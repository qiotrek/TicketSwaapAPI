using System.Security.Claims;

namespace TicketSwaapAPI.Helpers
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrinciple)
        {
            if (claimsPrinciple == null)
                throw new ArgumentNullException(nameof(claimsPrinciple));

            return claimsPrinciple.FindFirst(ClaimTypes.Sid)?.Value;

        }
    }
}
