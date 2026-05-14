using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Models.DTO
{
    public class ResetPasswordDto
    {
         [Required]
     [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string  Token{ get; set; }
        [Required]
         [MinLength(6)]
        public string NewPassword { get; set; }
         [Required]
    [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}