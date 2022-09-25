using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Auth.DTO.Requests
{
    public class UserUpdateRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
       
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }
}
