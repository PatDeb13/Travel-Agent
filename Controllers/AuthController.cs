using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
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
    }
}