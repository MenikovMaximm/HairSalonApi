using HairSalonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MastersController : Controller
    {
        private readonly HairSalonContext _context;

        public MastersController(HairSalonContext context)
        {
            _context = context;
        }

        // GET: api/masters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Master>>> GetMasters([FromQuery] string specialization)
        {
            var query = _context.Masters.AsQueryable();

            if (!string.IsNullOrEmpty(specialization))
                query = query.Where(m => m.Specialization == specialization);

            return await query.ToListAsync();
        }

        // POST: api/masters
        [HttpPost]
        public async Task<ActionResult<Master>> CreateMaster(Master master)
        {
            master.Id = Guid.NewGuid();
            _context.Masters.Add(master);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMasters), new { id = master.Id }, master);
        }

        // PUT: api/masters/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaster(Guid id, Master master)
        {
            if (id != master.Id) return BadRequest();
            _context.Entry(master).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!MasterExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/masters/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaster(Guid id)
        {
            var master = await _context.Masters.FindAsync(id);
            if (master == null) return NotFound();
            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MasterExists(Guid id) => _context.Masters.Any(e => e.Id == id);
    }
}
