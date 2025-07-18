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
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string malla, [FromForm] string asignatura)
                {
            try
         {
              var id = await _pdfService.UploadPdfAsync(file, malla, asignatura);
             return Ok(new { id });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}

          [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var pdfs = await _pdfService.GetAllPdfsAsync();
            return Ok(pdfs);
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
