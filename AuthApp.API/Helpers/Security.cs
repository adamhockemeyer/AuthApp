using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;


namespace AuthApp.API.Helpers
{
    /// <summary>
    /// https://blog.wille-zone.de/post/secure-azure-functions-with-jwt-token/
    /// and
    /// https://stackoverflow.com/a/39870281
    /// </summary>
    public static class Security
    {
        private static readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

        static Security()
        {
            //var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var issuer = "https://login.microsoftonline.com/common/v2.0";

            var documentRetriever = new HttpDocumentRetriever();
            documentRetriever.RequireHttps = issuer.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

            //var aa = typeof(Microsoft.IdentityModel.Logging.LogHelper);

            //Microsoft.IdentityModel.Logging.LogHelper.LogVerbose("test");

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{issuer}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever()
        );


        }

        public static async Task<ClaimsPrincipal> ValidateTokenAsync(AuthenticationHeaderValue value)
        {
            if (value?.Scheme != "Bearer")
            {
                return null;
            }

            var config = await _configurationManager.GetConfigurationAsync(CancellationToken.None);
            //var issuer = Environment.GetEnvironmentVariable("ISSUER");
            //var audience = Environment.GetEnvironmentVariable("AUDIENCE");

            var validationParameter = new TokenValidationParameters()
            {
                /*
                RequireSignedTokens = true,
                //ValidAudience = audience,
                ValidateAudience = false,
                //ValidIssuer = issuer,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKeys = config.SigningKeys
                */
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = false
            };

            ClaimsPrincipal result = null;
            var tries = 0;

            while (result == null && tries <= 1)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    result = handler.ValidateToken(value.Parameter, validationParameter, out var token);
                }
                catch (SecurityTokenSignatureKeyNotFoundException)
                {
                    // This exception is thrown if the signature key of the JWT could not be found.
                    // This could be the case when the issuer changed its signing keys, so we trigger a 
                    // refresh and retry validation.
                    _configurationManager.RequestRefresh();
                    tries++;
                }
                catch (SecurityTokenException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    return null;
                }
            }

            return result;
        }
    }
}
