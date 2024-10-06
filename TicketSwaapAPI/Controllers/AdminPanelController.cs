using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TicketSwaapAPI.Helpers;
using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Logic;
using TicketSwaapAPI.StoreModels;

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

        [Authorize(Roles = "User,Admin")]
        [HttpGet("MyNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<Notification>>> GetMyNotifications()
        {
            try
            {
                _logger.LogInformation("Trying to return user Notifications");
                List<Notification> result = await _adminPanelLogic.GetMyNotifications(this.User.GetUserId());
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("MyNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> CloseNotification(string id)
        {
            try
            {
                _logger.LogInformation("Trying to close user Notification");
                bool result = await _adminPanelLogic.CloseNotification(this.User.GetUserId(),id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
