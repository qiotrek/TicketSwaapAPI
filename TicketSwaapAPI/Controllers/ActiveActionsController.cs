using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSwaapAPI.Helpers;
using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Logic;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("[controller]")]
    public class ActiveActionsController:ControllerBase
    {
        private readonly IActiveActionsLogic _activeActionsLogic;
        private readonly ILogger<ActiveActionsController> _logger;

        public ActiveActionsController(IActiveActionsLogic activeActionsLogic, ILogger<ActiveActionsController> logger)
        {
            _activeActionsLogic = activeActionsLogic;
            _logger = logger;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetAction")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActiveActionViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<ActiveActionViewModel>> ActiveActionGet(string id)
        {
            try
            {
                _logger.LogInformation("Trying to return active action model");
                ActiveActionViewModel result = await _activeActionsLogic.GetActiveAction(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetActions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActiveActionModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<ActiveActionModel>>> ActiveActionsGet()
        {
            try
            {
                _logger.LogInformation("Trying to returnlist of active actions model");
                List<ActiveActionModel> result = await _activeActionsLogic.GetActiveActions();
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("Offert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActiveActionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ActiveActionModel>> AddNewOffert( string id, string sector, string place,string? mainOffertId)
        {

            try
            {
                ActiveActionModel action = await _activeActionsLogic.AddNewOffert(id, place, sector, this.User.GetUserId(), mainOffertId);

                return Ok(action);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("Offert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActiveActionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ActiveActionModel>> DeleteOffert(string actionId, string offertId)
        {

            try
            {
                ActiveActionModel action = await _activeActionsLogic.DeleteOffert(actionId, offertId, this.User.GetUserId());

                return Ok(action);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
