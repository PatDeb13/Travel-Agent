using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Models.DTO
{
    public class LoginResponseDto
    {
          public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Subsidiary { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
       
        public DateTime ExpiresAt { get; set; }
    }
}