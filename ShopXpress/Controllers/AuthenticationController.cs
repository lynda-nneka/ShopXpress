using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("login", Name = "login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }


        [HttpPost("create-account", Name = "create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterRequest request)
        {
            var response = await _authService.CreateAccount(request);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }


        [HttpPost("create-role", Name = "create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {

            await _authService.CreateRole(request);

            return Ok(new { message = "role created successfully" });
        }
    }
}
