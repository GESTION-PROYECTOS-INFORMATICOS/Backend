using AspNetCore.Identity.Mongo.Model;

namespace backGestion.Models
{
    public class Usuario : MongoUser<string>
    {
        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Nombre { get; set; } = null!;
    }
}
