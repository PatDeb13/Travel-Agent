using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Travel_Agent.Auth;
using Travel_Agent.Auth.AuthModel;
using Travel_Agent.Entities.Models;
using Travel_Agent.Entities.Models.Data;

namespace Travel_Agent.Auth.AuthServices
{
    public class AuthService :IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration ;
        private readonly IEmailService _emailService;
        private readonly IEmployee _employee;
    

        public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context
        ,RoleManager<IdentityRole> roleManager,IConfiguration configuration, IEmailService emailService, IEmployee employee  )
        {
            _userManager = userManager;
            _context = context;
            _roleManager =roleManager;
            _configuration =configuration;
            _emailService =emailService;
            _employee =employee;
        }

       public async Task<ResponseModelAuth<string>> RegisterNewUser([FromBody] RegisterDto payload)
        {
            var UserExists = await _userManager.FindByEmailAsync(payload.Email);
            if (UserExists !=null)
            {
                return new ResponseModelAuth<string>
            
                {
                    IsSuccessful = false,
                    Message ="User Already Exists"

                };              
    
            }
            var newUser = new ApplicationUser
            {
              FirstName = payload.FirstName,
              LastName = payload.LastName,
              Email = payload.Email,
              LineManager =payload.LineManager,
              Subsidiary = payload.Subsidiary,
              Level =payload.Level,
              Unit = payload.Unit,
              
            
            };
            var result = await _userManager.CreateAsync(newUser, payload.Password);
            if (!result.Succeeded)
            {
                return new ResponseModelAuth<string>
                {
                    IsSuccessful =false,
                    Message= "User could not be created!",

                };
                
            }
            
            var subject = "Account Created Successfully";
            var body = $"Hello {newUser.FirstName},\n\n" +
           "Your account has been successfully created.\n" +
           $"Email: {newUser.Email}\n\n" +
           "You can now log in to the system.\n\n" +
           "Best regards,\nTravel Agent Team";
           
            return new ResponseModelAuth<string>
            {
                IsSuccessful= true,
                Message= "$ User created",
                Data = newUser.EmployeeID
                
            };

        }
    }
}

    
    public interface IAuthService
    {
         Task<ResponseModelAuth<string>> RegisterNewUser(RegisterDto payload);
    }

  
