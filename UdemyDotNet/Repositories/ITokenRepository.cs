using Microsoft.AspNetCore.Identity;

namespace UdemyDotNet.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
