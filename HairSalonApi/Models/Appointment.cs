using System.Diagnostics.Metrics;

namespace HairSalonApi.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public Guid MasterId { get; set; }
        public Master Master { get; set; }
    }
}
