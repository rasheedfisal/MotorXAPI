using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Notification: BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string? ImgPath { get; set; }
    }
}
