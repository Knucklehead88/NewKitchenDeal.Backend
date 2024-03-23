using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        //private readonly IConfigurationSection _googleSettings;
        private readonly MyAwsCredentials _credentials;


        public TokenService(MyAwsCredentials credentials)
        {
            _credentials = credentials;
            //_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(credentials.TokenKey));
            //_googleSettings = _config.GetSection("Authentication:Google");
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.DisplayName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _credentials.TokenIssuer
                //Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuth externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = [_credentials.GoogleClientId]
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyFacebookToken(ExternalAuth externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = [_credentials.GoogleClientId]
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }



    }
}