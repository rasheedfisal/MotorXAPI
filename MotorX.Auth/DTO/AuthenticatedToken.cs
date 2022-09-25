using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Auth.DTO
{
    public class AuthenticatedToken
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
