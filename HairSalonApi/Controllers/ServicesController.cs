using HairSalonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly HairSalonContext _context;

        public ServicesController(HairSalonContext context)
        {
            _context = context;
        }

        // GET: api/services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices(
            [FromQuery] string category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(s => s.Category == category);

            if (minPrice.HasValue)
                query = query.Where(s => s.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(s => s.Price <= maxPrice);

            return await query.ToListAsync();
        }

        // POST: api/services
        [HttpPost]
        public async Task<ActionResult<Service>> CreateService(Service service)
        {
            service.Id = Guid.NewGuid();
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServices), new { id = service.Id }, service);
        }

        // PUT: api/services/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, Service service)
        {
            if (id != service.Id) return BadRequest();
            _context.Entry(service).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/services/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ServiceExists(Guid id) => _context.Services.Any(e => e.Id == id);
    }
}
