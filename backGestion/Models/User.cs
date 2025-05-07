using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backGestion.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get;set;}

    [EmailAddress]
    public string? EmailAddr {get;set;}

    [BsonElement("nombre")] //Esta línea hace que MongoDB entienda el campo "nombre"
    public string? Nombre { get; set; }

}