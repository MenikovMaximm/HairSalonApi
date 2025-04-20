using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/masters")]
    public class MastersController : Controller
    {
        private readonly HairSalonContext _context;

        public MastersController(HairSalonContext context)
        {
            _context = context;
        }

        // Получение всех мастеров
        [HttpGet]
        public async Task<IActionResult> GetAllMasters([FromQuery] string? specialization)
        {
            var query = _context.Masters.AsQueryable();

            if (!string.IsNullOrEmpty(specialization))
                query = query.Where(m => m.Major.Contains(specialization));

            var masters = await query.ToListAsync();
            return Ok(masters);
        }

        // Получение мастера по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaster(int id)
        {
            var master = await _context.Masters.FindAsync(id);
            if (master == null) return NotFound();
            return Ok(master);
        }

        // Создание мастера
        [HttpPost]
        public async Task<IActionResult> CreateMaster(MasterCreateDto masterDto)
        {
            var master = new Master
            {
                FirstName = masterDto.FirstName,
                Major = masterDto.Major
            };

            _context.Masters.Add(master);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaster), new { id = master.MasterId }, master);
        }

        // Обновление мастера
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaster(int id, MasterUpdateDto masterDto)
        {
            var master = await _context.Masters.FindAsync(id);
            if (master == null) return NotFound();

            if (!string.IsNullOrEmpty(masterDto.FirstName))
                master.FirstName = masterDto.FirstName;

            if (!string.IsNullOrEmpty(masterDto.Major))
                master.Major = masterDto.Major;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MasterExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Удаление мастера
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaster(int id)
        {
            var master = await _context.Masters.FindAsync(id);
            if (master == null) return NotFound();

            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MasterExists(int id)
        {
            return _context.Masters.Any(e => e.MasterId == id);
        }
    }
}