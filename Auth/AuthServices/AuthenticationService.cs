using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Travel_Agent.Entities.Models.Data;

namespace Travel_Agent.Auth.AuthServices
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration ;
    

        public AuthenticationService(UserManager<ApplicationUser> userManager, ApplicationDbContext context
        ,RoleManager<IdentityRole> roleManager,IConfiguration configuration )
        {
            _userManager = userManager;
            _context = context;
            _roleManager =roleManager;
            _configuration =configuration;
        }

       public async Task<Response> RegisterUser([FromBody] RegisterDto payload)
        {
            var user = new User
        }
    }

    public interface IAuthenticationService
    {
         Task<Response> RegisterUser([FromBody] RegisterDto payload);
    }

  
}