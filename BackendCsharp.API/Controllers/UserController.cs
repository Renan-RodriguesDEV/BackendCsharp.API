using BackendCsharp.API.DTOs;
using BackendCsharp.API.Entities;
using BackendCsharp.API.Repositories;
using BackendCsharp.API.Responses;
using BackendCsharp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult Get()
        {

            return Ok("Ok");
        }
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult Login([FromBody] UserRequests user)
        {
            try
            {

                UserEntity userDb = repository.Find(user.Username);
                bool check = passwordService.Verify(user.Password, userDb.Password);
                if (!check) { return Unauthorized("Acesso negado"); }
                UserResponse response = new UserResponse(Id: userDb.Id, Username: userDb.Username, CreatedAt: userDb.CreatedAt);
                Console.WriteLine(response.ToString());
                string token = jwtService.GenerateToken(userDb);
                return Ok(new TokenResponse{ Token=token });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserRequests user)
        {
            try
            {
                var response = repository.Save(user);
                return Created("", response);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return BadRequest();
            }


        }
    }
}
