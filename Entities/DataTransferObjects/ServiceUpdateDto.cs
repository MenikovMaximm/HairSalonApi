using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ServiceUpdateDto
    {
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Цена должна быть между 0.01 и 100000")]
        public decimal Price { get; set; }

        [StringLength(50, ErrorMessage = "Категория не должна превышать 50 символов")]
        public string Category { get; set; }
    }
}
