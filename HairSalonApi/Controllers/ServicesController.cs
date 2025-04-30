using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : Controller
    {
        private readonly HairSalonContext _context;

        public ServicesController(HairSalonContext context)
        {
            _context = context;
        }

        // Получение всех услуг
        [HttpGet]
        [Authorize(Roles = "Master,Client,Admin")]
        public async Task<IActionResult> GetAllServices([FromQuery] string? category, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var query = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(s => s.Category == category);

            if (minPrice.HasValue)
                query = query.Where(s => s.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(s => s.Price <= maxPrice.Value);

            var services = await query.ToListAsync();
            return Ok(services);
        }

        // Получение услуги по ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        // Создание услуги
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateService(ServiceCreateDto serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Price = serviceDto.Price,
                Category = serviceDto.Category
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
        }

        // Обновление услуги
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateService(int id, ServiceUpdateDto serviceDto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            if (!string.IsNullOrEmpty(serviceDto.Name))
                service.Name = serviceDto.Name;

            if (!string.IsNullOrEmpty(serviceDto.Description))
                service.Description = serviceDto.Description;

            if (serviceDto.Price > 0)
                service.Price = serviceDto.Price;

            if (!string.IsNullOrEmpty(serviceDto.Category))
                service.Category = serviceDto.Category;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Удаление услуги
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
