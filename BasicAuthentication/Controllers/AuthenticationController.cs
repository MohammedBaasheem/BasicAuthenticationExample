using BasicAuthentication.Data;
using BasicAuthentication.DTOs.Request;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Controllers
{
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly DBcontext _dbcontext;
        public AuthenticationController(DBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegesterAsync([FromBody] RegesterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var existingUser = await _dbcontext.Users
                .Where(user => user.Username == dto.Username || user.Email == dto.Email)
                .ToListAsync();

            if (existingUser.Any())
            {
                if (existingUser.Any(user => user.Username == dto.Username))
                {
                    return BadRequest("Username already exists.");
                }

                if (existingUser.Any(user => user.Email == dto.Email))
                {
                    return BadRequest("Email already registered.");
                }
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name,
            };
            await _dbcontext.Users.AddAsync(newUser);
            await _dbcontext.SaveChangesAsync();
            return Ok("Registration successful.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            var user = await _dbcontext.Users
                .Where(user => user.Username == dto.Username && user.Password == dto.Password)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Invalid credentials.");
            }
            return Ok("Login successful.");
        }


    }
}
