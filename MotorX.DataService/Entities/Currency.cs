﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Currency: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string CurrencyName { get; set; }
        [MaxLength(100)]
        public string? CurrencyNameAr { get; set; }
    }
}
