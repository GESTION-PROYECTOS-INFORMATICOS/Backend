using backGestion.Interfaces;
using backGestion.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RequestService : IRequestService
{
    private readonly IMongoCollection<Request> _requestCollection;

    public RequestService(IOptions<GMDatabaseSettings> gMDatabaseSettings)
    {
        var mongoClient = new MongoClient(gMDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(gMDatabaseSettings.Value.DatabaseName);

        _requestCollection = mongoDatabase.GetCollection<Request>(gMDatabaseSettings.Value.RequestsCollectionName);
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

    public async Task<List<Request>> GetAllRequestsAsync()
    {
        return await _requestCollection.Find(_ => true).ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(string id, string status)
    {
        var update = Builders<Request>.Update
            .Set(r => r.Status, status)
            .Set(r => r.ApprovalDate, DateTime.UtcNow);

        var result = await _requestCollection.UpdateOneAsync(
            r => r.Id == id,
            update
        );

        return result.ModifiedCount > 0;
    }
}
