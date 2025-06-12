using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Request
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } // id de solicitud

    [BsonElement("DocumentId")]
    public string DocumentId { get; set; } // id del documento asociado

    [BsonElement("RequestReason")] //motivo de la solicitd
    public string RequestReason { get; set; }

    [BsonElement("RequestedBy")]
    public string RequestedBy { get; set; } // Nombre o ID del usuario que solicita

    [BsonElement("Status")]
    public string Status { get; set; } = "Pending"; //Pending, Approved, Rejected -> indican
                                                        //estados de aprobación

    [BsonElement("RequestDate")]
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;

     [BsonElement("FileData")]
        public byte[] FileData { get; set; }

    [BsonElement("ApprovalDate")]
    public DateTime? ApprovalDate { get; set; }
}
