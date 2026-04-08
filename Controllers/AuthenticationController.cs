using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Travel_Agent.Auth;

namespace Travel_Agent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthenticationController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Register([FromBody] RegisterDto payload)
        {
            var response = await _service.RegisterNewUser(payload);
            if(response != null)
            {
                return BadRequest($" User not Creted/available");
            
            }
            return Ok(response);
        }

    }
}