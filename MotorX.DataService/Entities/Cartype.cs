﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Cartype: BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string TypeName { get; set; }
        [MaxLength(400)]
        public string? TypeNameAr { get; set; }
        [MaxLength(400)]
        public string? ImgPath { get; set; }
    }
}
