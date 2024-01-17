using Core.Entities;
using Core.Entities.Identity;
using Google.Apis.Auth;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuth externalAuth);
    }
}