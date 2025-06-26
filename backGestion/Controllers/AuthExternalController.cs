// backGestion/Controllers/AuthExternalController.cs
// Controlador para sincronizar usuarios de Microsoft (externos) con MongoDB y gestionar roles

using Microsoft.AspNetCore.Mvc;
using backGestion.Models; // Asegúrate de que tu modelo Usuario y Rol estén definidos aquí
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace backGestion.Controllers
{
    [ApiController] // Este atributo indica que es un controlador de API
    [Route("api/[controller]")] // Define la ruta base para este controlador, que será /api/AuthExternal
    public class AuthExternalController : ControllerBase // Renombrado a AuthExternalController para evitar conflictos
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager; 

        public AuthExternalController(UserManager<Usuario> userManager, RoleManager<Rol> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Modelo para el cuerpo de la solicitud POST
        public class SyncUserRequest
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// Sincroniza un usuario de Microsoft (enviado por NextAuth) con la base de datos de MongoDB.
        /// Crea el usuario si no existe y le asigna un rol por defecto.
        /// Retorna la información del usuario incluyendo su ID de MongoDB y sus roles.
        /// </summary>
        /// <param name="request">Objeto que contiene el email y el nombre del usuario.</param>
        /// <returns>Un JSON con el ID del usuario, email, nombre y roles.</returns>
        [HttpPost("sync-microsoft-user")] // La ruta completa para este método será /api/AuthExternal/sync-microsoft-user
        public async Task<IActionResult> SyncMicrosoftUser([FromBody] SyncUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "El email es requerido para la sincronización del usuario." });
            }

            Console.WriteLine($"Backend: Intentando sincronizar usuario con email: {request.Email}");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            var roles = new List<string>();

            if (existingUser == null)
            {
                // El usuario no existe en MongoDB, crearlo.
                var nuevoUsuario = new Usuario
                {
                    UserName = request.Email, // Usar email como UserName o un valor único adecuado
                    Email = request.Email,
                    Nombre = request.Name ?? request.Email.Split('@')[0] // Usar nombre proporcionado o derivar del email
                };

                var createResult = await _userManager.CreateAsync(nuevoUsuario);
                if (!createResult.Succeeded)
                {
                    Console.Error.WriteLine($"Backend: Error al crear el usuario {request.Email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    return StatusCode(500, new { message = $"Error al crear el usuario: {string.Join(", ", createResult.Errors.Select(e => e.Description))}" });
                }
                Console.WriteLine($"Backend: Usuario {request.Email} creado exitosamente.");

                // Asignar rol por defecto (ej. "Alumno")
                var roleExists = await _roleManager.RoleExistsAsync("Alumno");
                if (!roleExists)
                {
                    Console.Error.WriteLine("Backend: El rol 'Alumno' no existe. Por favor, asegúrate de que se cree al iniciar la aplicación.");
                } else {
                    var roleResult = await _userManager.AddToRoleAsync(nuevoUsuario, "Alumno");
                    if (!roleResult.Succeeded)
                    {
                        Console.Error.WriteLine($"Backend: Error al asignar rol 'Alumno' a {request.Email}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                    roles.Add("Alumno");
                    Console.WriteLine($"Backend: Rol 'Alumno' asignado a {request.Email}.");
                }
                
                existingUser = nuevoUsuario;
            }
            else
            {
                // El usuario ya existe, obtener sus roles actuales
                roles = (await _userManager.GetRolesAsync(existingUser)).ToList();
                Console.WriteLine($"Backend: Usuario {request.Email} ya existe. Roles actuales: {string.Join(", ", roles)}");

                // Opcional: Actualizar el nombre del usuario si ha cambiado
                if (existingUser.Nombre != request.Name && !string.IsNullOrEmpty(request.Name))
                {
                    existingUser.Nombre = request.Name;
                    await _userManager.UpdateAsync(existingUser);
                    Console.WriteLine($"Backend: Nombre de usuario {request.Email} actualizado a {request.Name}.");
                }
            }

            // Devolver la información del usuario y sus roles
            // NextAuth recibirá esto en el callback signIn
            return Ok(new
            {
                id = existingUser.Id, // ID de MongoDB para este usuario
                email = existingUser.Email,
                name = existingUser.Nombre,
                roles = roles // Lista de roles del usuario
            });
        }
    }
}
