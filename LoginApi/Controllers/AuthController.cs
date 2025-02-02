using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IStoreRepository storeRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {

            var user = await _userRepository.GetUsernameAsync(userLogin.UserName);
            if (user == null || user.Password != userLogin.Password)
            {
                return Unauthorized("Usuario ou senha invalidos");
            }


            if (user.IsActive == false)
            {
                return Unauthorized("Usuario inativo");
            }
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }
            var store = await _storeRepository.GetStoreByCodeAsync(user.StoreCode);
            if (store == null)
            {
                return BadRequest("Loja não existe");
            };

            await _userRepository.AddUserAsync(user);
            store.UsersId.Add(user.IdUser);
            await _storeRepository.UpdateStoreAsync(store);
            return Ok("Usuario cadastrado com sucesso");


        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("getall")]
        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string userName)
        {
            var user = await _userRepository.GetUsernameAsync(userName);
            await _userRepository.DeleteUserAsync(user.UserName);
            return Ok("Usuario deletado com sucesso");
        }



        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);



        }


    }
}
