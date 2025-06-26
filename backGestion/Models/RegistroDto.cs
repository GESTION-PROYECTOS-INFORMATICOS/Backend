namespace backGestion.Models
{
    public class RegistroDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; } // Ej: "Profesor"
    }
}
