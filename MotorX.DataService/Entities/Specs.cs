using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Specs:BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string SpecsName { get; set; }
        [MaxLength(400)]
        public string? SpecsNameAr { get; set; }
    }
}
