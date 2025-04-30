using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public ClientShortInfoDto Client { get; set; }
        public MasterWithAppointmentsDto Master { get; set; }
        public ServiceInfoDto Service { get; set; }
    }

    public class ClientShortInfoDto
    {
        public string FirstName { get; set; }
        public string Phone { get; set; }
    }

    public class MasterWithAppointmentsDto
    {
        public string FirstName { get; set; }
        public string Major { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<AppointmentShortInfoDto> Appointments { get; set; }

    }

    public class ServiceInfoDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    public class AppointmentShortInfoDto
    {
        public DateTime Date { get; set; }
    }
}
