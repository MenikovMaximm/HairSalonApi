using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Entities.DataTransferObjects;
using HairSalonApi;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using HairSalonApi.Services;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly HairSalonContext _context;
        private readonly JwtService _jwtService;

        public AuthController(HairSalonContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ClientRegisterDto clientDto)
        {
            if (await _context.Clients.AnyAsync(c => c.Email == clientDto.Email))
                return BadRequest("Пользователь с таким email уже существует");

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                Email = clientDto.Email,
                Phone = clientDto.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(clientDto.Password),
                Role = "Client"
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(client);

            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ClientLoginDto clientDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == clientDto.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(clientDto.Password, client.Password))
                return Unauthorized("Неверный email или пароль");

            var token = _jwtService.GenerateToken(client);

            return Ok(new
            {
                Token = token
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Вы успешно вышли из системы" });
        }
    }
}
