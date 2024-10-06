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
    public class UserPanelController:ControllerBase
    {
        private readonly IUserPanelLogic _userPanelLogic;
        private readonly ILogger<UserPanelController> _logger;

        public UserPanelController(IUserPanelLogic adminPanelLogic, ILogger<UserPanelController> logger)
        {
            _userPanelLogic = adminPanelLogic;
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
                List<OffertViewModel> result = await _userPanelLogic.GetMyOfferts(this.User.GetUserId());
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
                List<Notification> result = await _userPanelLogic.GetMyNotifications(this.User.GetUserId());
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
                bool result = await _userPanelLogic.CloseNotification(this.User.GetUserId(), id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("Contact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> AddQuestion([FromBody] ProblemsAndQuestionsModel model)
        {
            try
            {
                _logger.LogInformation("Trying to add question");
                bool result = await _userPanelLogic.AddQuestion(this.User.GetUserId(), model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("Contact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> CloseQuestion(string id)
        {
            try
            {
                _logger.LogInformation("Trying to add question");
                bool result = await _userPanelLogic.CloseQuestion(this.User.GetUserId(), id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
