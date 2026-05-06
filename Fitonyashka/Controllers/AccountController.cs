using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fitonyashka.ViewModels.Account;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fitonyashka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _cfg;

        public AccountController(IUserService userService, IConfiguration cfg) {
            _userService = userService;
            _cfg = cfg;
        }

        // GET: api/Account
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel) {
            //TODO: add validation
            //if (!ModelState.IsValid) {
            //    return Page();
            //}

            var userDto = new UserDto {
                UserName = loginViewModel.Username,
                Password = loginViewModel.Password
            };
            var result = _userService.Login(userDto.UserName, userDto.Password);
            if (!result) {
                return BadRequest("You entered wrong username or password");
            }

            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userDto.UserName)
            };

            var token = new JwtSecurityToken(
              issuer: jwt["Issuer"],
              audience: jwt["Audience"],
              claims: claims,
              expires: DateTime.UtcNow.AddHours(1),
              signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenStr });
        }

        // POST: api/Account
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] RegisterUserViewModel registerViewModel)
        {
            //TODO: add validation
            //if (!ModelState.IsValid) {
            //    return Page();
            //}

            var userDto = new UserDto {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
                Password = registerViewModel.Password
            };
            var result = _userService.Create(userDto);
            if (!result.IsSuccess) {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Account/5

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
