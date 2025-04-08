using HairSalonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly HairSalonContext _context;

        public AppointmentController(HairSalonContext context)
        {
            _context = context;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments(
            [FromQuery] DateTime? date,
            [FromQuery] Guid? clientId,
            [FromQuery] Guid? masterId)
        {
            var query = _context.Appointments.AsQueryable();

            if (date.HasValue)
                query = query.Where(a => a.Date.Date == date.Value.Date);

            if (clientId.HasValue)
                query = query.Where(a => a.ClientId == clientId);

            if (masterId.HasValue)
                query = query.Where(a => a.MasterId == masterId);

            return await query.ToListAsync();
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {
            appointment.Id = Guid.NewGuid();
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAppointments), new { id = appointment.Id }, appointment);
        }

        // PUT: api/appointments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, Appointment appointment)
        {
            if (id != appointment.Id) return BadRequest();
            _context.Entry(appointment).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/appointments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AppointmentExists(Guid id) => _context.Appointments.Any(e => e.Id == id);
    }
}