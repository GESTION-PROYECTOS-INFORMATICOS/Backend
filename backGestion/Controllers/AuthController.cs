// backGestion/Controllers/AuthController.cs

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backGestion.Models;
using Microsoft.AspNetCore.Identity;

namespace backGestion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Usuario> _userManager;

        public AuthController(IConfiguration config, UserManager<Usuario> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet("signin-microsoft")]
        public async Task<IActionResult> MicrosoftCallback()
        {
            Console.WriteLine("Callback recibido");
            // Autentica el contexto con el esquema de cookies.
            // El middleware de OpenIdConnect ya habrá procesado la respuesta de Microsoft
            // y establecido un Principal en el esquema de cookies si todo fue bien.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                // Si la autenticación falla o no hay principal, devuelve Unauthorized
                return Unauthorized();
            }

            // Extrae los claims del Principal obtenido de la autenticación
            var claims = result.Principal.Claims.ToList();
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (emailClaim == null)
            {
                return BadRequest("No se pudo obtener el email del proveedor externo.");
            }

            var email = emailClaim.Value;
            var nombre = nameClaim?.Value ?? "SinNombre"; // Nombre predeterminado si no se encuentra

            // Busca el usuario en tu base de datos MongoDB
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                // Si el usuario no existe, créalo
                var nuevoUsuario = new Usuario
                {
                    UserName = email,
                    Email = email,
                    Nombre = nombre
                };

                var createResult = await _userManager.CreateAsync(nuevoUsuario);
                if (!createResult.Succeeded)
                {
                    // Manejo de errores si la creación del usuario falla
                    return StatusCode(500, $"Error al crear el usuario: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
                
                // Asigna un rol por defecto (ej. "Alumno")
                var roleResult = await _userManager.AddToRoleAsync(nuevoUsuario, "Alumno");
                if (!roleResult.Succeeded)
                {
                    // Manejo de errores si la asignación de rol falla
                    return StatusCode(500, $"Error al asignar rol al usuario: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
                existingUser = nuevoUsuario;
            }

            // Prepara los claims que se incluirán en tu JWT personalizado
            var jwtClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id!), // ID del usuario en tu DB
                new Claim(ClaimTypes.Name, existingUser.Nombre!), // Nombre del usuario
                new Claim(ClaimTypes.Email, existingUser.Email!) // Email del usuario
            };

            // Añade los roles del usuario a los claims del JWT
            var roles = await _userManager.GetRolesAsync(existingUser);
            jwtClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Genera el JWT
            var token = GenerarJwt(jwtClaims);

            // Redirige al frontend con el token en la URL como un query parameter
            // ¡IMPORTANTE! Asegúrate de que esta URL sea la URL REAL de tu frontend de Next.js
            // y que tu componente en el frontend esté configurado para leer 'token' de los query parameters.
            return Redirect($"http://localhost:3000/Auth?token={token}");
        }

        private string GenerarJwt(IEnumerable<Claim> claims)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
