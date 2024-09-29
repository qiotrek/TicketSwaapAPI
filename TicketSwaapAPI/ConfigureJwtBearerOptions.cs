using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace TicketSwaapAPI
{
    public partial class Startup
    {
        public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
        {
            public IConfiguration Configuration { get; }
            //private readonly ILoggerManager _logger;
            public ConfigureJwtBearerOptions(IConfiguration configuration)
            {

                Configuration = configuration;
            }


            public void Configure(string name, JwtBearerOptions options)
            {
                RSA rsa = RSA.Create();


                /*
                RSA rsa = RSA.Create(4096);
                string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                */
                //var paramsPub = rsa.ExportParameters(false);
                //var paramsPrv = rsa.ExportParameters(true);

                rsa.ImportRSAPublicKey(Convert.FromBase64String(Configuration.GetValue<string>("JwtPublicSigningKey")), out _);

                var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecretKey")));
                //var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
                //var encryptionCredentials = new EncryptingCredentials(secret, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);


                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    TokenDecryptionKey = secret,
                    //TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("test_secret")),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("Issuer"),
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("Audience"),
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            }

            public void Configure(JwtBearerOptions options)
            {
                throw new NotImplementedException();
            }
        }




    }
}
