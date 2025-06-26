using AspNetCore.Identity.Mongo.Model;

namespace backGestion.Models
{
    public class Rol : MongoRole<string>
    {
        public Rol()
        {
            Id = Guid.NewGuid().ToString();  // Genera un nuevo Id único
        }

        public Rol(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
