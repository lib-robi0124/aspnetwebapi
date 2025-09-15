using Avenga.NotesApp.Dtos.UserDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Authorize] // this means that every method will require authorization
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous] // no token needed(we cannot be logged in before registration)
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                Log.Information("Registration attempt for user: {Username}", registerUserDto.Username);
                _userService.RegisterUser(registerUserDto);
                Log.Information("User {Username} successfully registered.", registerUserDto.Username);
                return StatusCode(StatusCodes.Status201Created, "User Created");
            }
            catch (UserDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred during registration for user: {Username}. Error: {ErrorMessage}", registerUserDto.Username, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")] // no token needed (we cannot be logged in before login)
        public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                string token = _userService.LoginUser(loginUserDto);
                Log.Information("User {Username} successfully logged in.");
                return Ok(token);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred during login for user: {Username}. Error: {ErrorMessage}", e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpGet] // successfull token saving needed in order to access this method
        public IActionResult TestIfLoggedIn()
        {
            return Ok("Successfully accessed after authentication!");
        }
    }
}
