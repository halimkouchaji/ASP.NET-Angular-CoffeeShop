using Microsoft.AspNetCore.Identity;


namespace CoffeeShop.API.Repositories
{
    public interface IAuthRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
