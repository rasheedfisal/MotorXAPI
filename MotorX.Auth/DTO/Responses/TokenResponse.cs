﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Auth.DTO.Responses
{
    public class TokenResponse: AuthResultResponse
    {
        public string RefreshToken { get; set; }
    }
}
