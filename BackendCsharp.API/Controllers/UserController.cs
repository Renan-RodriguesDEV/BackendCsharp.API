using BackendCsharp.API.DTOs;
using BackendCsharp.API.Entities;
using BackendCsharp.API.Repositories;
using BackendCsharp.API.Responses;
using BackendCsharp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BackendCsharp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository repository;
        private readonly PasswordService passwordService;
        private readonly JwtService jwtService;
        public UserController(UserRepository repository, PasswordService passwordService, JwtService jwtService)
        {
            this.repository = repository;
            this.passwordService = passwordService;
            this.jwtService = jwtService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(new
            {
                userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
            });
        }
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public IActionResult Login([FromBody] UserRequests user)
        {
            try
            {
                UserEntity? userDb = repository.Find(user.Email);
                if (userDb == null)
                {
                    return Unauthorized("Credenciais inválidas.");
                }

                bool check = passwordService.Verify(user.Password, userDb.Password);
                if (!check) { return Unauthorized("Credenciais inválidas."); }

                string token = jwtService.GenerateToken(userDb);
                return Ok(new TokenResponse { Token = token });
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(UserResponse), 201)]
        public IActionResult Register([FromBody] UserRequests user)
        {
            try
            {
                var response = repository.Save(user);
                return Created(string.Empty, response);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }


        }
    }
}
