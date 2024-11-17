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
        [HttpPost("GetActionsList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActiveActionModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<ActiveActionModel>>> ActiveActionsListPost([FromBody] string[] actionIds)
        {
            try
            {
                _logger.LogInformation("Próba zwrócenia listy aktywnych działań");
                List<ActiveActionModel> result = await _activeActionsLogic.GetActiveActions(actionIds);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("UserFavorites")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetUserFavoritesActions()
        {
            try
            {
                _logger.LogInformation("Trying to returnlist of fav actions");
                List<string> result = await _activeActionsLogic.GetUserFavoritesActions(this.User.GetUserId());
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPatch("UserFavorites")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<bool>> ToggleUserFavoriteAction(string actionId)
        {
            try
            {
                _logger.LogInformation("Trying to update of fav actions");
                bool result = await _activeActionsLogic.UpdateUserFavoriteAction(this.User.GetUserId(), actionId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("Offert/{mainOffertId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OffertModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<OffertModel>>> GetIntrestedOfferts(string mainOffertId)
        {

            try
            {
                List<OffertModel> offerts = await _activeActionsLogic.GetIntrestedOfferts(mainOffertId);
                return Ok(offerts);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
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
