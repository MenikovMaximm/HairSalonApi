using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Client 
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // В реальном проекте храним хэш
        public string Role { get; set; } = "Client"; // По умолчанию
        public List<Appointment> Appointments { get; set; }
    }
}
