using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Models.DTO
{
    public class RegisterDto
    {
         public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public Level Level { get; set; }
        public string Subsidiary { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string? LineManager { get; set; }="N";
    }
}