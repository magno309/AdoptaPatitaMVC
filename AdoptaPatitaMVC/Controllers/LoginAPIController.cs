using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdoptaPatitaMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginAPIController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginAPI login){
            if(ModelState.IsValid){
                var usuario = await _userManager.FindByEmailAsync(login.Email);
                if(usuario == null){
                    return BadRequest(new {
                        Error = "Intento de autenticación inválido"
                    });
                }

                bool correcto = await _userManager.CheckPasswordAsync(usuario, login.Password);
                if(!correcto){
                    return BadRequest(new {
                        Error = "Intento de autenticación inválido"
                    });
                }
                
                var jwtToken = GenerateJwtToken(usuario);
                return Ok(new {
                    Token = jwtToken
                });
            }
            return BadRequest();
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            // jwtTokenHandler para crear los tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Obtener secret key de appsettings
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            // Los Claims son propiedades del usuario para generar su token            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("id", user.Id),
                    new Claim("email", user.Email),
                }),
                
                // Tiempo de vida del token
                Expires = DateTime.UtcNow.AddHours(1),

                // Adición de la información del algoritmo de encriptación 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };            

            // Crear token con las descripciones dadas
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            Console.WriteLine("UTC Login: "+DateTime.UtcNow);
            
            // Escribir el token             
            return jwtTokenHandler.WriteToken(token);
        }
    }
}