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
    public class AdminController : ControllerBase
    {

        private readonly JwtGenerator _jwtGenerator;
        private readonly IAdminLogic _adminLogic;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminLogic adminLogic, IConfiguration configuration, ILogger<AdminController> logger)
        {
            _jwtGenerator = new JwtGenerator(configuration);
            _configuration = configuration;
            _logger = logger;
            _adminLogic = adminLogic;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> AcceptActionProposition(string id)
        {
         
            _logger.LogInformation("Received AcceptActionProposition request");

            try
            {
                bool result = await _adminLogic.AcceptNewAction(id,this.User.GetUserId());
                   

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

        [Authorize(Roles = "Admin")]
        [HttpPut("Reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> RejectActionProposition(string id)
        {
            try
            {
                bool result = await _adminLogic.RejectNewAction(id);


                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }           
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ProblemsAndQuestions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProblemsAndQuestionsModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<List<ProblemsAndQuestionsModel>>> GetProblemsAndQuestions()
        {
            try
            {
                List<ProblemsAndQuestionsModel> result = await _adminLogic.GetProblemsAndQuestions();


                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("ProblemsAndQuestions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<bool>> OpenProblemsAndQuestions(string id)
        {
            try
            {
                bool result = await _adminLogic.OpenProblemsAndQuestions(this.User.GetUserId(),id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }

    
}

