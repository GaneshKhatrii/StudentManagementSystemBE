using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystemBE.Data;
using StudentManagementSystemBE.DTOs.AuthDTOs;
using StudentManagementSystemBE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementSystemBE.Services.Implementations
{
    public class AuthService
    {
        public readonly StudentManagementDbContext _context;
        public readonly IConfiguration _config;

        // Constructor to initialize the database context and configuration
        // IConfiguration is a built-in interface which is used to access settings like JWT secret keys, token expiration times and etc.
        // StudentManagementDbContext is used to interact with the database for user authentication and authorization
        public AuthService(StudentManagementDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> Register(RegisterDto userData)
        {
            var userExists = await _context.Teachers.AnyAsync(teacher => teacher.TeacherName == userData.Name);
            if (userExists) return false; // User already exists, registration failed

            var teacher = new Teacher()
            {
                TeacherName = userData.Name,

                // Hash the password before storing it in the database
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userData.Password),

                // Default role for new users 
                Role = "Teacher"
            };

            // Add the new teacher to the database context
            _context.Teachers.Add(teacher);

            // Save changes to the database asynchronously because it may take some time, especially if the database is large or under heavy load. This allows the application to remain responsive while the operation completes.
            await _context.SaveChangesAsync();

            // Registration successful
            return true;
        }

        public async Task<string?> Login(LoginDto userData)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(teacher => teacher.TeacherName == userData.Name);
            if (teacher == null) return null;
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userData.Password, teacher.PasswordHash);
            if (!isPasswordValid) return null;

            return GenerateJwtToken(teacher);
        }

        private string GenerateJwtToken(Teacher teacher)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, teacher.TeacherId.ToString()),
                new Claim(ClaimTypes.Name, teacher.TeacherName),
                new Claim(ClaimTypes.Role, teacher.Role)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
