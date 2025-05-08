using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using backGestion.Models;

namespace backGestion.Interfaces
{
    public interface IPdfService
    {
        Task<string> UploadPdfAsync(IFormFile file);
        Task<PdfFile> GetPdfAsync(string id);
    }
}
