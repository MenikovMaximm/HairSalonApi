using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ClientUpdateDto
    {
        [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string Phone { get; set; }

        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; }
    }
}
