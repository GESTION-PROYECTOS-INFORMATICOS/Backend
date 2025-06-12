using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backGestion.Models
{
    public class PdfFile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("FileName")]
    public string FileName { get; set; }

    [BsonElement("ContentType")]
    public string ContentType { get; set; }

    [BsonElement("FileData")]
    public byte[] FileData { get; set; }

    [BsonElement("Malla")]
    public string Malla { get; set; }

    [BsonElement("Asignatura")]
    public string Asignatura { get; set; }

    [BsonElement("FechaSubida")]
    public DateTime FechaSubida { get; set; }
}
}
