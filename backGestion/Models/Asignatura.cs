using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backGestion.Models;
[BsonIgnoreExtraElements]
public class Asignatura
{
 
    [BsonElement("nombre")]
    public string Nombre { get; set; } // nombre asignatura

    [BsonElement("mallaId")]
    [BsonRepresentation(BsonType.String)]
    public string MallaId { get; set; }  //Referencia a la malla (Ej: "malla_2024_ajuste")

    [BsonElement("semestre")]
    public int Semestre { get; set; }

    [BsonElement("codigo")]

    public string Codigo { get; set; } // codigo asignatura

}