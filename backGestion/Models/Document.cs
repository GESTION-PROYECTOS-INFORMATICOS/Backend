using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backGestion.Models;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get;set;}

    public string? ReferenceTo{get;set;}
}
