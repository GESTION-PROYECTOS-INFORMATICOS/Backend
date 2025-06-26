using backGestion.Models;
using backGestion.Services;
using backGestion.Interfaces;
using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.Mongo; // Asegúrate de tener esta referencia
using Microsoft.Extensions.DependencyInjection; // Para CreateScope
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración personalizada (colecciones)
builder.Services.Configure<GMDatabaseSettings>(
    builder.Configuration.GetSection("GMDatabase"));

// --- Configuración de Identity con MongoDB ---
// Ahora usando string como tipo de clave
builder.Services.AddIdentity<Usuario, Rol>()
    .AddMongoDbStores<Usuario, Rol, string>(mongoIdentityOptions =>
    {
        mongoIdentityOptions.ConnectionString = builder.Configuration.GetConnectionString("MongoDB");
    })
    .AddDefaultTokenProviders();

// Servicios personalizados
builder.Services.AddSingleton<UsersService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IMallaService, MallaService>();
builder.Services.AddScoped<IAsignaturaService, AsignaturaService>();

// Configuración de CORS para permitir comunicación con el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger (Swashbuckle)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Crear roles al iniciar la app
await CrearRolesAsync(app.Services);

app.Run();

// Método para crear roles si no existen
async Task CrearRolesAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Rol>>();

    string[] roles = new[] { "Profesor", "Alumno", "Admin" };

    foreach (var rolNombre in roles)
    {
        var existe = await roleManager.RoleExistsAsync(rolNombre);
        if (!existe)
        {
            var resultado = await roleManager.CreateAsync(new Rol { Name = rolNombre });
            if (!resultado.Succeeded)
            {
                Console.WriteLine($"Error al crear el rol {rolNombre}");
            }
        }
    }
}
