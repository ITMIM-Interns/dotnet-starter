using Identity.Entity.Models;

namespace Identity.BLL.Abstractions.Externals
{
    public interface ITokenService
    {
        string CreateAccessToken(User user, string[] roles=null);
    }
}
