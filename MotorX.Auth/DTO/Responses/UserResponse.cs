using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Auth.DTO.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
       
        public string Email { get; set; }
     
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }

    }
}
