using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthRepository authRepository;
        public AuthController(UserManager<IdentityUser> userManager, IAuthRepository authRepository)
        {
            this.userManager = userManager;
            this.authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CoffeeShop.API.Models.Dto.RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var result = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (result.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Length > 0)
                {
                    result = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (!result.Succeeded)
                    {
                        await userManager.DeleteAsync(identityUser);
                        return BadRequest(result.Errors);
                    }

                }
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CoffeeShop.API.Models.Dto.LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);
            if (user != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (isPasswordValid)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var token = authRepository.CreateJWTToken(user, roles.ToList());
                        return Ok(new { Token = token });

                    }
          
                }
            }
            return BadRequest("Invalid username or password");
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var usersDTO = userManager.Users
                .AsNoTracking()
                .Select(user => new CoffeeShop.API.Models.Dto.LoginRequestDto { Username = user.UserName
                })
                .ToList();

            if (usersDTO.Count == 0)
                return NotFound("No users found");

            return Ok(usersDTO);
        }
    }
}
