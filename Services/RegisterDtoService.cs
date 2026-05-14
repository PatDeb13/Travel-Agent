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
        private readonly IEmailService _emailService;

        public RegisterDtoService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration =configuration;
            _emailService = emailService;
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
              if (user == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful = true,
                    Message = "If your email is registered, you will receive a password reset link."
                };
            }
            if (!user.IsActive)
            {
                return new ResponseDto<string>
                {
                    IsSuccessful= false,
                    Message= " The email has been deactivated by adminstrator! construct your Admin!!!"

                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

              var encodedToken = WebUtility.UrlEncode(token);
              var encodedEmail = WebUtility.UrlEncode(dto.Email);

              var resetLink =  $"https://localhost:5232/api/Auth/reset-password?email={encodedEmail}&token={encodedToken}";
              var subject ="Reset Your Password-Travel Agent";
              var body = GenerateResetPasswordEmail(user.FirstName, resetLink);
             try
    {
        await _emailService.SendEmail(dto.Email, subject, body, isHtml: true);
        
        return new ResponseDto<string>
        {
            IsSuccessful = true,
            Message = "If your email is registered, you will receive a password reset link."
        };
    }
    catch (Exception ex)
    {
        return new ResponseDto<string>
        {
            IsSuccessful = false,
            Message = ex.Message
        };
    }
}

      private string GenerateResetPasswordEmail(string firstName, string resetLink)
{
    return @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email</title>
    <style>
        * {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: Arial, Helvetica, sans-serif;
}

body {
  background: #ffffff;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}
.email-container {
  background: #ffffff;
  border: #e2e8f0;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  padding: 20px;
}
.logoBox {
  width: 100px;
  object-fit: contain;
}
.logo {
  width: 100%;
  height: 100%;
}

.content-box {
  border: 3px;
  padding: 20px;
}

.title {
  font-size: 22px;
  font-weight: bold;
  color: #333;
  margin-bottom: 20px;
  text-transform: uppercase;
}
.text {
  font-size: 14px;
  color: #555;
  line-height: 1.8;
  margin-bottom: 20px;
}

.btn {
  display: inline-block;
  background: #0d47a1;
  color: #fff;
  text-decoration: none;
  padding: 12px 30px;
  border-radius: 6px;
  font-size: 14px;
  font-weight: bold;
  margin-bottom: 20px;
}

.btn:hover {
  background: #1565c0;
}

.footer {
  font-size: 14px;
  color: #666;
}

    </style>
 </head>
 <body>

    <div class=""email-container"">

        <!-- Logo -->
         <div class=""logoBox"">

             <img src=""Image/Frame 1686560347.png"" alt=""Logo"" class=""logo"">
         </div>

        <!-- Content -->
        <div class=""content-box"">

            <h2 class=""title"">
                PASSWORD RESET REQUEST
            </h2>

            <p class=""text"">
                Dear " + (string.IsNullOrWhiteSpace(firstName) ? "User" : firstName) + @",
                  </p>

            <p class=""text"">
                We received a request to reset your password. Click the button below to create a new password.
            </p>

            <a href=""" + resetLink + @""" class=""btn"">
                Reset Password
            </a>

            <p class=""footer"">
                If you didn't request this, please ignore this email.
            </p>
            <p class=""footer"">
                Thanks.
            </p>
        </div>
    </div>
 </body>
 </html>";
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
            if (!user.IsActive)
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
                new Claim("Subsidiary", user.Subsidiary.ToString()?? ""),
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