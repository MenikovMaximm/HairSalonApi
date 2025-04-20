using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly HairSalonContext _context;

        public ClientsController(HairSalonContext context)
        {
            _context = context;
        }

        // Получение всех клиентов (только для администратора)
        [HttpGet]
        [Authorize(Roles = "Admin")] // В реальном проекте добавить авторизацию
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        // Получение клиента по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        // Обновление данных клиента
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientUpdateDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            // Обновляем только те поля, которые были переданы
            if (!string.IsNullOrEmpty(clientDto.FirstName))
                client.FirstName = clientDto.FirstName;

            if (!string.IsNullOrEmpty(clientDto.Email))
                client.Email = clientDto.Email;

            if (!string.IsNullOrEmpty(clientDto.Phone))
                client.Phone = clientDto.Phone;

            if (!string.IsNullOrEmpty(clientDto.Password))
                client.Password = BCrypt.Net.BCrypt.HashPassword(clientDto.Password);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }

        // Удаление клиента (только для администратора)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
