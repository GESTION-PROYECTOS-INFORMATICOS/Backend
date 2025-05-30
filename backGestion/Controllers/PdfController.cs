using Microsoft.AspNetCore.Mvc;
using backGestion.Services;
using backGestion.Models;
using backGestion.Interfaces;

namespace backGestion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                var id = await _pdfService.UploadPdfAsync(file);
                return Ok(new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var pdf = await _pdfService.GetPdfAsync(id);
            if (pdf == null)
                return NotFound();

            return File(pdf.FileData, pdf.ContentType, pdf.FileName);
        }
    }
}
