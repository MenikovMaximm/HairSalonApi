using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentUpdateDto
    {
        [Required(ErrorMessage = "ID мастера обязательно")]
        public int MasterId { get; set; }

        [Required(ErrorMessage = "ID услуги обязательно")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Дата обязательна")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Время обязательно")]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Статус обязателен")]
        [StringLength(20, ErrorMessage = "Статус не должен превышать 20 символов")]
        public string Status { get; set; }
    }
}
