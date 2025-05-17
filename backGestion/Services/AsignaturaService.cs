using backGestion.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace backGestion.Services;
public class AsignaturaService : IAsignaturaService
{
    private readonly IMongoCollection<Asignatura> _asignaturaCollection;

    public AsignaturaService(IOptions<GMDatabaseSettings> gMDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            gMDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            gMDatabaseSettings.Value.DatabaseName);

        _asignaturaCollection = mongoDatabase.GetCollection<Asignatura>(
            gMDatabaseSettings.Value.AsignaturasCollectionName);
    }

   public async Task<IEnumerable<Asignatura>> GetAsignaturaMallaSemestre(string malla_id, int semestre)
        {
            if (!string.IsNullOrWhiteSpace(malla_id) && semestre > 0)
            {
                return await _asignaturaCollection
                    .Find(a => a.MallaId == malla_id && a.Semestre == semestre)
                    .ToListAsync();
            }
            else
            {
                Console.WriteLine("Debe ingresar valores válidos.");
                return [];
            }
        }


}