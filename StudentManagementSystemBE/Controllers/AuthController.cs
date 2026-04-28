using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystemBE.DTOs.AuthDTOs;
using StudentManagementSystemBE.DTOs.Common;
using StudentManagementSystemBE.Services.Implementations;

namespace StudentManagementSystemBE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        // Dependency injection of the AuthService to access authentication logic like registration and login.
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto userData)
        {
            var result = await _authService.Register(userData);

            if (!result)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "User already exists",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "User registered successfully.",
                Data = null
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userData)
        {
            var token = await _authService.Login(userData);

            if (token == null)
            {
                return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 401,
                        Message = "Invalid credentials",
                        Data = null
                    });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Login successful",
                Data = new { token }
            });
        }

        [HttpGet("generate-hash")]
        public IActionResult GenerateHash(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(hash);
        }
    }
}
