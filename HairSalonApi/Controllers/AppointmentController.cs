using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonApi.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly HairSalonContext _context;

        public AppointmentController(HairSalonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments(
       [FromQuery] DateTime? date,
       [FromQuery] int? clientId,
       [FromQuery] int? masterId,
       [FromQuery] string? status)
        {
            var query = _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Master)
                .Include(a => a.Service)
                .AsQueryable();

            if (date.HasValue)
                query = query.Where(a => a.Date == date.Value);

            if (clientId.HasValue)
                query = query.Where(a => a.ClientId == clientId.Value);

            if (masterId.HasValue)
                query = query.Where(a => a.MasterId == masterId.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            var appointments = await query.ToListAsync();
            return Ok(appointments);
        }

        // Получение записи по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Master)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        // Создание записи
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentCreateDto appointmentDto)
        {
            // Проверка существования связанных сущностей
            if (!await ClientExists(appointmentDto.ClientId))
                return BadRequest("Клиент не найден");

            if (!await MasterExists(appointmentDto.MasterId))
                return BadRequest("Мастер не найден");

            if (!await ServiceExists(appointmentDto.ServiceId))
                return BadRequest("Услуга не найдена");

            // Проверка доступности мастера
            var isMasterAvailable = !await _context.Appointments
                .AnyAsync(a => a.MasterId == appointmentDto.MasterId
                            && a.Date == appointmentDto.Date
                            && a.Time == appointmentDto.Time
                            && a.Status != "Отменен");

            if (!isMasterAvailable)
                return BadRequest("Мастер уже занят в это время");

            var appointment = new Appointment
            {
                ClientId = appointmentDto.ClientId,
                MasterId = appointmentDto.MasterId,
                ServiceId = appointmentDto.ServiceId,
                Date = appointmentDto.Date,
                Time = appointmentDto.Time,
                Status = "Запланирован"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
        }

        // Обновление записи
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentUpdateDto appointmentDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Проверка существования связанных сущностей
            if (!await MasterExists(appointmentDto.MasterId))
                return BadRequest("Мастер не найден");

            if (!await ServiceExists(appointmentDto.ServiceId))
                return BadRequest("Услуга не найдена");

            // Проверка доступности мастера (исключая текущую запись)
            var isMasterAvailable = !await _context.Appointments
                .AnyAsync(a => a.MasterId == appointmentDto.MasterId
                            && a.Date == appointmentDto.Date
                            && a.Time == appointmentDto.Time
                            && a.Status != "Отменен"
                            && a.AppointmentId != id);

            if (!isMasterAvailable)
                return BadRequest("Мастер уже занят в это время");

            appointment.MasterId = appointmentDto.MasterId;
            appointment.ServiceId = appointmentDto.ServiceId;
            appointment.Date = appointmentDto.Date;
            appointment.Time = appointmentDto.Time;
            appointment.Status = appointmentDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Удаление записи
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }

        private async Task<bool> ClientExists(int id)
        {
            return await _context.Clients.AnyAsync(e => e.ClientId == id);
        }

        private async Task<bool> MasterExists(int id)
        {
            return await _context.Masters.AnyAsync(e => e.MasterId == id);
        }

        private async Task<bool> ServiceExists(int id)
        {
            return await _context.Services.AnyAsync(e => e.ServiceId == id);
        }
    }
}