using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Travel_Agent.Data;
using Travel_Agent.Models;
using Travel_Agent.Models.DTO;

namespace Travel_Agent.Services
{
    public class RegisterDtoService:IRegisterDtoServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterDtoService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
    public async Task<ResponseDto<string>> CreatUser(RegisterDto dto)
        {
            if (dto ==null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful= false,
                    Message = "Invalid Data"
                };
            }
            if (string.IsNullOrWhiteSpace(dto.Email)|| string.IsNullOrWhiteSpace(dto.EmployeeId) )
            {
                return new ResponseDto<string>
                {
                    IsSuccessful=false,
                    Message ="Email and EmployeeId required "
                };
            }
            var newUser = await _userManager.FindByEmailAsync(dto.Email);
        
        if (newUser == null)
        {
             newUser = new ApplicationUser
            {
                Email=dto.Email,
                EmployeeId=dto.EmployeeId,
                FirstName =dto.FirstName,
                LastName=dto.LastName,
                Level=dto.Level,
                LineManager =dto.LineManager,
                Subsidiary =dto.Subsidiary,
                Unit =dto.Unit
            };
        }
            await _userManager.CreateAsync(newUser, dto.Password);
            var save = await _userManager.AddToRoleAsync(newUser, "User");
            return new ResponseDto<string>
            {
                IsSuccessful=true,
                Message="User Created successful",
            }  ;
        

        } 

    }

    public interface IRegisterDtoServices
    {
        Task<ResponseDto<string>> CreatUser(RegisterDto dto);

    }
}

