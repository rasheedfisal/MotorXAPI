using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Auth.DTO.Responses
{
    public class UserLoginResponse: AuthResultResponse  
    {
        public string UserName { get; set; }
    }
}
