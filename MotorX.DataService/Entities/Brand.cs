using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Brand: BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(400)]
        public string? NameAr { get; set; }
        [MaxLength(200)]
        public string? LogoPath { get; set; }
    }
}
