using backGestion.Models;
using backGestion.Services;
using backGestion.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<GMDatabaseSettings>(
    builder.Configuration.GetSection("GMDatabase"));

//para colecciones
builder.Services.AddSingleton<UsersService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IMallaService, MallaService>();
builder.Services.AddScoped<IAsignaturaService, AsignaturaService>();
//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// colocar esto antes de maocontrollers
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
