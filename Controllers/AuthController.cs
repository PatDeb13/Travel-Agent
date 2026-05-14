using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Travel_Agent.Models.DTO;
using Travel_Agent.Services;

namespace Travel_Agent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterDtoServices _services;

        public AuthController(IRegisterDtoServices services)
        {
            _services = services;
        }

        [HttpPost("Create-user")]
        public async Task<IActionResult> CreateNewUser([FromBody]RegisterDto dto)
        {
            var response = await _services.CreatUser(dto);
            if(response ==null)
            {
                return BadRequest("");
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            var response = await _services.LoginUser(dto);
            if (response ==null)
            {
                return BadRequest("Not successful");
            }
            return Ok(response);
        }

    
        [HttpPost ("forgot-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _services.ForgotPassword(dto);
            if(!result.IsSuccessful)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _services.ResetPassword(dto);
            if (!result.IsSuccessful)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

}
}