using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class MasterCreateDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Специализация обязательна")]
        [StringLength(100, ErrorMessage = "Специализация не должна превышать 100 символов")]
        public string Major { get; set; }
    }
}
