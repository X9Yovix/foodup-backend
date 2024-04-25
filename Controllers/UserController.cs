using Backend.DTOS;
using Backend.Helpers;
using Backend.Models;
using Backend.Pattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _uow.UserRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(DTOS.LoginRequest loginRequest)
        {
            var user = await _uow.UserRepository.GetUserByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BadRequest(new { Status = "Error", Message = "Invalid email or password" });
            }

            var token = JwtHelper.GenerateJwtToken(user);
            var userDto = new LoginResponse
            {
                Message = "Logged in successfully",
                Token = token
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(DTOS.RegisterRequest registerRequest)
        {
            var existingUser = await _uow.UserRepository.GetUserByEmail(registerRequest.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Status = "Error", Message = "Email already exists" });
            }

            registerRequest.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            var user = new User()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Gender = registerRequest.Gender,
                Role = "user"
            };
            var data = await _uow.UserRepository.Register(user);
            await _uow.SaveChangesAsync();
            return Ok(data);
        }
    }
}
