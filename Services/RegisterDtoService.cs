using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Travel_Agent.Data;
using Travel_Agent.Models;
using Travel_Agent.Models.DTO;

namespace Travel_Agent.Services
{
    public class RegisterDtoService : IRegisterDtoServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RegisterDtoService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration =configuration;
        }

    public async Task<ResponseDto<string>> CreatUser(RegisterDto dto)
        {
            // Check if dto is null
            if (dto == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = "Invalid Data"
                };
            }

            //  Check required fields
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.EmployeeId))
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = "Email and EmployeeId required"
                };
            }

            //  Check if password is provided
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = "Password is required"
                };
            }

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = "User with this email already exists"
                };
            }

            //  Check if EmployeeId is unique
            var existingEmployee = _userManager.Users.FirstOrDefault(u => u.EmployeeId == dto.EmployeeId);
            if (existingEmployee != null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = "EmployeeId already exists"
                };
            }

            // Create new user
            var newUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Level = dto.Level,  
                LineManager = dto.LineManager,
                Subsidiary = dto.Subsidiary,
                Unit = dto.Unit,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                
            };

            //  user creation 
            var createResult = await _userManager.CreateAsync(newUser, dto.Password);
            
            if (!createResult.Succeeded)
            {
                // Get all error messages from Identity
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = $"Failed to create user: {errors}"
                };
            }

            //   "User" role exists before adding
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            //   role assignment succeeded
            var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
            
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return new ResponseDto<string>
                {
                    IsSuccessful = false,
                    Message = $"User created but role assignment failed: {roleErrors}"
                };
            }
            return new ResponseDto<string>
            {
                IsSuccessful = true,
                Message = "User Created Successfully",
                Data = newUser.Email  
            };
        }
    
     public async Task<ResponseDto<string>> ForgotPassword(ForgetPasswordDto dto)
        {
            if(dto ==null || string.IsNullOrWhiteSpace(dto.Email))
            {
                return  new ResponseDto<string>
                {
                    IsSuccessful= false,
                    Message ="Email required"
                };
            }
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user.IsActive)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful=true,
                    Message= "Check your email for reset Password link"
                };
            }
            if (!user.IsActive)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful= false,
                    Message= " The email has been deactivated by adminstrator! construt your Admin!!!"

                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

              var encodedToken = WebUtility.UrlEncode(token);
              var encodedEmail = WebUtility.UrlEncode(dto.Email);

              var resetLink = $"https://yourapp.com/reset-password?email={encodedEmail}&token={encodedToken}";
                 Console.WriteLine($"Password reset token for {user.Email}: {token}");
    Console.WriteLine($"Reset link: {resetLink}");
    
    return new ResponseDto<string>
    {
        IsSuccessful = true,
        Message = "If your email is registered, you will receive a password reset link."
      };
   }

    public async Task<ResponseDto<string >> ResetPassword(ResetPasswordDto dto)
        {
            if(dto == null || string.IsNullOrWhiteSpace(dto.Email)|| string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return new ResponseDto<string>
                {
                    IsSuccessful=false,
                    Message ="Email, Token, and new pasword required"
                };
            }
            if(dto.NewPassword != dto.ConfirmPassword)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful=false,
                    Message= "New password and confirm Password does not match"

                };
            }
            var user =await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful =false,
                    Message= "Invalid reset request"

                };
            }
            var decodedToken = WebUtility.UrlDecode(dto.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",",result.Errors.Select(e=> e.Description));
                return new ResponseDto<string>
                {
                    IsSuccessful =false,
                    Message =$"Password reset failed:{errors}"
                };
            }
            await _userManager.UpdateSecurityStampAsync(user);
            return new ResponseDto<string>
            {
                IsSuccessful=true,
                Message="Password has been reset successfully. You can now login with your new password."
            };
        }    
    
    public async Task<ResponseDto<LoginResponseDto>> LoginUser(LoginDto dto)
        {
            if(dto ==null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new ResponseDto<LoginResponseDto>
                {
                    IsSuccessful=false,
                    Message="Email and Passwords are required"
                };
            
            }
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if(user == null)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    IsSuccessful=false,
                    Message ="Invalid email or password"
                };
            }
            if (user.IsActive)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    IsSuccessful= false,
                    Message ="Your account has been deactivated.Please contact administrator."
                };
            }
           var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
           if(!passwordValid)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    IsSuccessful=false,
                    Message="Invalid email or password"
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            return new ResponseDto<LoginResponseDto>
            {
                IsSuccessful=true,
                Message ="Login successful",
                Data = new LoginResponseDto
                {
                 Token = token,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeId = user.EmployeeId,
                Level = user.Level.ToString(),
                Subsidiary = user.Subsidiary,
                Unit = user.Unit,
                Roles = roles.ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(1)
                }
            };
        }
    
    public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            // Create claims (information stored in the token)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FirstName", user.FirstName ?? ""),
                new Claim("LastName", user.LastName ?? ""),
                new Claim("EmployeeId", user.EmployeeId ?? ""),
                new Claim("Level", user.Level.ToString()),
                new Claim("Subsidiary", user.Subsidiary ?? ""),
                new Claim("Unit", user.Unit ?? "")
            };

             foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured")));
            
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1); 

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

              return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
     
    public interface IRegisterDtoServices
    {
        Task<ResponseDto<string>> CreatUser(RegisterDto dto);
        Task<ResponseDto<LoginResponseDto>> LoginUser(LoginDto dto);
        Task<ResponseDto<string>> ForgotPassword(ForgetPasswordDto dto);  
        Task<ResponseDto<string>> ResetPassword(ResetPasswordDto dto);
    }
}