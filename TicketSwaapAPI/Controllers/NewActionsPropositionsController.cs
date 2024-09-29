using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSwaapAPI.Helpers;
using TicketSwaapAPI.Services.Logic;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.Services.Security;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("[controller]")]
    public class NewActionsPropositionsController:ControllerBase
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly INewActionPropositionlogic _newActionPropositionlogic;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NewActionsPropositionsController> _logger;

        public NewActionsPropositionsController(JwtGenerator jwtGenerator, INewActionPropositionlogic newActionPropositionlogic, IConfiguration configuration, ILogger<NewActionsPropositionsController> logger)
        {
            _jwtGenerator = jwtGenerator;
            _configuration = configuration;
            _logger = logger;
            _newActionPropositionlogic = newActionPropositionlogic;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NewActionProposition>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<List<NewActionProposition>>> NewActionsPropositionsGet()
        {
            try
            {
                _logger.LogInformation("Trying to return list of new actions propositions");
                List<NewActionProposition> result = await _newActionPropositionlogic.GetList();
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("IsRequested")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> NewActionsPropositionIsRequested(string id)
        {
            try
            {
                bool result = await _newActionPropositionlogic.CheckProposition(id,this.User.GetUserId());
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewActionProposition))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddNewActionProposition([FromBody] NewActionPropositionModel model)
        {
            if (model == null)
            {
                _logger.LogError("Invalid model: null");
                return BadRequest("Invalid model.");
            }

            _logger.LogInformation("Received AddNewActionProposition request with ID: {Id}, Name: {Name}, Date: {Date}", model.Id, model.Name, model.Date);

            try
            {
                NewActionProposition result = await _newActionPropositionlogic.AddNewActionRequest(
                    model.Id,
                    model.Name,
                    model.Img,
                    model.Date,
                    this.User.GetUserId());

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in AddNewActionProposition");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}

public class NewActionPropositionModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Img { get; set; }
}
