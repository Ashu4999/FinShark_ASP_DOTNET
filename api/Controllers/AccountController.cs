using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var foundUser = await _userManager.FindByEmailAsync(registerDto.Email);

                if (foundUser != null) 
                    return Conflict("User already exists");

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded) {
                    var roles = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roles.Succeeded)
                        return Ok(
                            new NewUserDto {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        ); 
                    else 
                        return StatusCode(500, roles.Errors);
                } else 
                    return StatusCode(500, createdUser.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}