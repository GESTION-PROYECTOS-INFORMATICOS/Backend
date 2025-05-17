using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace backGestion.Models;

public class Malla
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }

    [BsonElement("nombre")]
    public string Nombre { get; set; }
    
    [BsonElement("cantidadSemestres")]
    public int CantidadSemestres { get; set; }

}