using HairSalonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly HairSalonContext _context;

        public ClientsController(HairSalonContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return client;
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            client.Id = Guid.NewGuid();
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        // PUT by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, Client client)
        {
            if (id != client.Id) return BadRequest();
            _context.Entry(client).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ClientExists(Guid id) => _context.Clients.Any(e => e.Id == id);
    }
}
