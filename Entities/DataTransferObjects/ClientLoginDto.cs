using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ClientLoginDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }
}
