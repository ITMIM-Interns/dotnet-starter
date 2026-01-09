using MiniApp.Models.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Models
{
    public sealed class User :BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string? Image { get; set; }
        public string? LastVerificationCode { get; set; }
    }
}
