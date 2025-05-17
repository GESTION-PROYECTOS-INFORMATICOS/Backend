using System.Data.Common;
using backGestion.Models;
using backGestion.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace backGestion.Services;
public class MallaService : IMallaService
{
    private readonly IMongoCollection<Malla> _mallaCollection;
    public MallaService(IOptions<GMDatabaseSettings> gMDatabaseSettings)
    {
        var mongoClient = new MongoClient(
                gMDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                gMDatabaseSettings.Value.DatabaseName);

            _mallaCollection = mongoDatabase.GetCollection<Malla>(
                gMDatabaseSettings.Value.MallasCollectionName);
    }

    public async Task<IEnumerable<Malla>> GetAllMallas()
    {
        return await _mallaCollection.Find(_ => true).ToListAsync();
    }
}