using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Year: BaseEntity
    {
        [Required]
        public int YearName { get; set; }
    }
}
