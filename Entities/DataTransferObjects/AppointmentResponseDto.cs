using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; }

        public ClientSimpleDto Client { get; set; }
        public MasterSimpleDto Master { get; set; }
        public ServiceSimpleDto Service { get; set; }
    }

    public class ClientSimpleDto
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
    }

    public class MasterSimpleDto
    {
        public int MasterId { get; set; }
        public string FirstName { get; set; }
        public string Major { get; set; }
    }

    public class ServiceSimpleDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
