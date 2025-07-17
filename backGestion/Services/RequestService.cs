using backGestion.Interfaces;
using backGestion.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class RequestService : IRequestService
{
    private readonly IMongoCollection<Request> _requestCollection;

    public RequestService(IOptions<GMDatabaseSettings> gMDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            gMDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            gMDatabaseSettings.Value.DatabaseName);

        _requestCollection = mongoDatabase.GetCollection<Request>(
            gMDatabaseSettings.Value.RequestsCollectionName);
    }

    public async Task<string> CreateRequestAsync(Request r)
    {
        if (r == null)
            return "Solicitud inválida.";

        try
        {
            await _requestCollection.InsertOneAsync(r);
            return "Solicitud enviada correctamente.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al enviar solicitud: {e.Message}");
            return "No se pudo enviar la solicitud.";
        }
    }
        
        ///

}