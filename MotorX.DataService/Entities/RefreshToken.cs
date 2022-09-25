using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class RefreshToken: BaseEntity
    {
        public string UserId { get; set; } // userId when Logged in
        public string Token { get; set; }
        public string JwtId { get; set; } // the Id generated when a Jwt token is requested
        public bool IsUsed { get; set; } // to make sure the token is only used once
        public bool IsRevoked { get; set; } // make sure they are valid
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
