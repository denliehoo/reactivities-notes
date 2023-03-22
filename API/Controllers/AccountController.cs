using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// we separate identity from the rest of the application
// hence we derive from controllerbase instead of BaseApiController

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase

    {
        // remember: UserManager type is from AspNetCore.Identity
        // The generic type is AppUser of which we created
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (result)
            {
                return CreateUserObject(user);
            }
            // if result is wrong (i.e. wrong password)
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username Taken");
                return ValidationProblem();
                /* ModelState.AddModelError returns an object with an which makes it easier
                 to work with on our frontend 
                { //....,    
                    error: {
                        "username" : [
                            "Username Taken"
                        ]
                    }
                } */
            }
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email Taken");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded) // if successuflly registered, log the user in
            {
                return CreateUserObject(user);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);

        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }

}