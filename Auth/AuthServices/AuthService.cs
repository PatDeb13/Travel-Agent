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

       public async Task<ResponseModelAuth<RegisterDto>> RegisterNewUser(RegisterDto payload)
        {
            var UserExists = await _userManager.FindByEmailAsync(payload.Email);
            if (UserExists !=null)
            {
                return new ResponseModelAuth<RegisterDto>
            
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
              LineManager= payload.LineManager,
              Level =payload.Level,
              Subsidiary =payload.Subsidiary,
              Unit =payload.Unit,
             // SecurityStamp = Guid.NewGuid().ToString(),
              EmployeeID = await _employee.GenerateEmployeeId(),

            
            };
            var result = await _userManager.CreateAsync(newUser, payload.Password);
            if (!result.Succeeded)
            {
                return new ResponseModelAuth<RegisterDto>
                {
                    IsSuccessful =false,
                    Message= "User could not be created!",

                };
            }
            
        var subject = "Account Created Successfully";

        var body = $@"
        Hello {newUser.FirstName},

        Your account has been successfully created.

        Employee ID: {newUser.EmployeeID}
        Email: {newUser.Email}

        You can now log in.

        Best regards,Travel Agent Team";

            await _emailService.SendEmailAsync(newUser.Email, subject, body);
            return new ResponseModelAuth<RegisterDto>
            {
                IsSuccessful= true,
                Message= "$ User created"
                
                
            };

        }
    }
}

    
    public interface IAuthService
    {
         Task<ResponseModelAuth<RegisterDto>> RegisterNewUser(RegisterDto payload);
    }

  
