using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace CoffeeShop.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration configuration;
        
        public AuthRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var ceredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: ceredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
