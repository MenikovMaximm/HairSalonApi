using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Master
    {
        public int MasterId { get; set; }
        public string FirstName { get; set; }
        public string Major { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
