using MongoDB.Driver;
using backGestion.Models;
using Microsoft.AspNetCore.Http;
using backGestion.Interfaces;
using Microsoft.Extensions.Options;

namespace backGestion.Services
{
    public class PdfService : IPdfService
    {
        private readonly IMongoCollection<PdfFile> _pdfCollection;

        public PdfService(IOptions<GMDatabaseSettings> gMDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                gMDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                gMDatabaseSettings.Value.DatabaseName);

            _pdfCollection = mongoDatabase.GetCollection<PdfFile>(
                gMDatabaseSettings.Value.PdfsCollectionName);
        }

        public async Task<string> UploadPdfAsync(IFormFile file, string malla, string asignatura)
        {
            if (file == null || file.Length == 0 || !file.ContentType.Contains("pdf"))
                throw new ArgumentException("Archivo inválido.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var pdf = new PdfFile
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileData = fileBytes,
                Malla = malla,
                Asignatura = asignatura,
                FechaSubida = DateTime.UtcNow
            };

            await _pdfCollection.InsertOneAsync(pdf);
            return pdf.Id;
        }


        public async Task<PdfFile> GetPdfAsync(string id)
        {
            return await _pdfCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        
        public async Task<List<PdfFile>> GetAllPdfsAsync()
{
    return await _pdfCollection.Find(_ => true).ToListAsync();
}
    }
}
