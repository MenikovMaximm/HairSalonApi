using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int MasterId { get; set; }
        public Master Master { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // "Запланирован", "Выполнен", "Отменен"
    }
}
