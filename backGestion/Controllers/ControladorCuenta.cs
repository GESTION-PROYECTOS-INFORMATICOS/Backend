using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using backGestion.Models;

namespace backGestion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorCuenta : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;

        public ControladorCuenta(UserManager<Usuario> userManager, RoleManager<Rol> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroDto dto)
        {
            // Verifica si el rol es válido
            if (!await _roleManager.RoleExistsAsync(dto.Rol))
                return BadRequest($"El rol '{dto.Rol}' no existe.");

            // Crear el usuario
            var user = new Usuario
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nombre = dto.Nombre
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Asignar el rol
            await _userManager.AddToRoleAsync(user, dto.Rol);

            return Ok($"Usuario {dto.Email} registrado con rol {dto.Rol}.");
        }
    }
}
