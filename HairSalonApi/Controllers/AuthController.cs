using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Entities.DataTransferObjects;
using HairSalonApi;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly HairSalonContext _context;

        public AuthController(HairSalonContext context)
        {
            _context = context;
        }

        // Регистрация
        [HttpPost("register")]
        public async Task<IActionResult> Register(ClientRegisterDto clientDto)
        {
            if (await _context.Clients.AnyAsync(c => c.Email == clientDto.Email))
                return BadRequest("Пользователь с таким email уже существует");

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                Email = clientDto.Email,
                Phone = clientDto.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(clientDto.Password)
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok(new { client.ClientId, client.FirstName, client.Email });
        }

        // Авторизация
        [HttpPost("login")]
        public async Task<IActionResult> Login(ClientLoginDto clientDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == clientDto.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(clientDto.Password, client.Password))
                return Unauthorized("Неверный email или пароль");

            // В реальном проекте здесь должна быть генерация JWT токена
            return Ok(new { client.ClientId, client.FirstName, client.Email });
        }

        // Выход (заглушка, в реальном проекте нужно инвалидировать токен)
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok("Вы успешно вышли из системы");
        }
    }
}
