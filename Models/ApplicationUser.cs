using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Travel_Agent.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public Level Level { get; set; }
        public string Subsidiary { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string LineManager { get; set; } ="N";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        
    }

    public enum Level
    {
        Junior,
        Senior
    }
}