using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using backGestion.Models;

namespace backGestion.Interfaces
{
    public interface IPdfService
    {
        Task<string> UploadPdfAsync(IFormFile file, string malla, string asignatura);

        Task<PdfFile> GetPdfAsync(string id);
        Task<List<PdfFile>> GetAllPdfsAsync();
    }
}
