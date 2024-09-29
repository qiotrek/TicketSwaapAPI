using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSwaapAPI.Helpers;
using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Logic;

namespace TicketSwaapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("[controller]")]
    public class AdminPanelController:ControllerBase
    {
        private readonly IAdminPanelLogic _adminPanelLogic;
        private readonly ILogger<AdminPanelController> _logger;

        public AdminPanelController(IAdminPanelLogic adminPanelLogic, ILogger<AdminPanelController> logger)
        {
            _adminPanelLogic = adminPanelLogic;
            _logger = logger;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("MyOfferts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OffertViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<OffertViewModel>>> GetMyOfferts()
        {
            try
            {
                _logger.LogInformation("Trying to return user offerts");
                List<OffertViewModel> result = await _adminPanelLogic.GetMyOfferts(this.User.GetUserId());
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
