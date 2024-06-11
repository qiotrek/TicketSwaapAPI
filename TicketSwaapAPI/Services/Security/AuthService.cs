using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Security
{
    public class AuthService
    {

    }

    public class JwtGenerator
    {

        readonly string _audience;
        readonly string _issuser;
        readonly RsaSecurityKey _key;
        readonly string _secret;
        public JwtGenerator(IConfiguration configuration)
        {
            string jwtPrivateSigningKey = configuration.GetValue<string>("JwtPrivateSigningKey");
            _secret = configuration.GetValue<string>("SecretKey");
            _audience = configuration.GetValue<string>("Audience");
            _issuser = configuration.GetValue<string>("Issuer");

            RSA privateRSA = RSA.Create();
            privateRSA.ImportRSAPrivateKey(Convert.FromBase64String(jwtPrivateSigningKey), out _);
            _key = new RsaSecurityKey(privateRSA);
        }



        public Tuple<string, long> CreateUserAuthToken(UserModel user)
        {


            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            //var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var encryptionCredentials = new EncryptingCredentials(secretKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Name, user.Name.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };


            DateTime tomorrow = DateTime.Today.AddDays(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _audience,
                Issuer = _issuser,
                Subject = new ClaimsIdentity(claims),

                Expires = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0, DateTimeKind.Utc),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256),
                EncryptingCredentials = encryptionCredentials //new EncryptingCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("KPtest12")), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256)
            };



            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return new Tuple<string, long>(tokenHandler.WriteToken(token), ((DateTimeOffset)tokenDescriptor.Expires).ToUnixTimeSeconds());
            //return tokenHandler.CreateEncodedJwt(tokenDescriptor);
        }
    }
}
