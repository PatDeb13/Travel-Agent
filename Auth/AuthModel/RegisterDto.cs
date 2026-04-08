using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Travel_Agent.Auth
{
    public class RegisterDto :IdentityUser
    {
        [Required]
        public string  EmployeeID {get; set;}
        public string  Email {get; set;}
        public string Password{get; set;}
        public string Level {get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string Subsidiary{get; set;}
        public string Unit{get; set;}
        public string LineManager{get; set;}

    }
} 