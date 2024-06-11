using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSwaapAPI.Models;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.Services.Security;
using TicketSwaapAPI.StoreModels;


namespace TicketSwaapAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {



        private readonly JwtGenerator _jwtGenerator;
        private readonly IUserRepository _userRep;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;


        public UserController(IConfiguration configuration, IUserRepository userRepository, ILogger<UserController> logger)
        {
            _configuration = configuration;
            _jwtGenerator = new JwtGenerator(configuration);
            _userRep = userRepository;
            _logger = logger;

        }



        ///<summary>
        /// Returns a token with user data
        ///</summary>
        ///<response code="200">Returns a token with user data</response>
        ///<response code="401">That is, the client must authenticate itself to get the requested response.</response>
        ///<response code="403"> Does not have access rights to the content</response>
        [AllowAnonymous]
        [HttpPost("/.auth")]
        [HttpPost(".auth")]
        [HttpPost("/api/.auth")]
        public async Task<ActionResult<LogonModel>> Authenticate([FromBody] AuthenticateModel data)
        {
            _logger.LogInformation("Trying to return LogonModel.");
            var ttt = this.HttpContext;
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration.GetValue<string>("GoogleAudience") }
                };
                var tast = GoogleJsonWebSignature.ValidateAsync(data.Token, settings);

                GoogleJsonWebSignature.Payload payload = tast.Result;

                //var user1 = await _fsSvc.Set<UserModel>(_fsTabs.UsersTableName, payload.Email, new UserModel { Id=payload.Email, Email = payload.Email, Name = payload.Name });
                //var user2 = await _fsSvc.Set<UserModel>(_fsTabs.UsersTableName, "k.piekutowski@auchan.pl", new UserModel { Id = payload.Email, Email = "k.piekutowski@auchan.pl", Name = "Konrad Piekutowski" });


                UserModel user = await _userRep.Get(payload.Email);



                if (user != null)
                {

                    Tuple<string, long> token = _jwtGenerator.CreateUserAuthToken(user);

                    var logonModel = new LogonModel
                    {
                        UserId = user.Id,
                        AccessToken = token.Item1,
                        AccessTokenExpirationTime = token.Item2,
                        Email = user.Email,
                        Name = user.Name,
                        Role = user.Role

                    };

                    return Ok(logonModel);
                }
                else
                {
                    _logger.LogError($"UserExists");
                    return StatusCode(403);
                }


            }

            catch (Exception eex)
            {
                if (eex.InnerException is InvalidJwtException)
                {
                    //TODO: logger eex.InnerException.Message
                    _logger.LogError($"Something went wrong: {eex}");
                    return StatusCode(401);

                }

                throw;
            }



        }

        [AllowAnonymous]
        [HttpGet("/.authClients")]
        [HttpGet(".authClients")]
        [HttpGet("/api/.authClients")]
        public async Task<ActionResult<List<AuthClientModel>>> AuthClients()
        {
            await Task.Delay(500);
            List<AuthClientModel> result = new List<AuthClientModel>();
            _logger.LogInformation("Trying to return AuthClients.");
            string googleClientId = _configuration.GetValue<string>("GoogleAudience");
            string environment = _configuration.GetValue<string>("Environment", "Empty");

            result.Add(new AuthClientModel { ClientType = "Google", ClientId = googleClientId, Env = environment });

            _logger.LogInformation("Trying to return AuthClients.");
            return Ok(result);
        }



    }
}
