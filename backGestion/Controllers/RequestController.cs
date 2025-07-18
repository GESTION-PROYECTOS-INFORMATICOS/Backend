using Microsoft.AspNetCore.Mvc;
using backGestion.Services;
using backGestion.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
namespace backGestion.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IPdfService _pdfService;

    public RequestController(IRequestService requestService,IPdfService pdfService)
    {
        _requestService = requestService;
        _pdfService = pdfService;
    }

    [HttpPost("with-document")]
public async Task<IActionResult> PostRequestWithDocument([FromForm] IFormFile file, [FromForm] string requestReason, [FromForm] string requestedBy,
    [FromForm] string malla, [FromForm] string asignatura)
{
    if (file == null || file.Length == 0)
        return BadRequest("Debe subir un documento PDF.");

    try
    {
        // 1. Subir el documento primero (usando tu PdfService).
        var pdfId = await _pdfService.UploadPdfAsync(file, malla, asignatura);

        // 2. Crear la solicitud con el ID del documento.
        var request = new Request
        {
            DocumentId = pdfId,
            RequestReason = requestReason,
            RequestedBy = requestedBy,
            Status = "Pending",
            RequestDate = DateTime.UtcNow
        };

        await _requestService.CreateRequestAsync(request);

        return Ok(new { message = "Solicitud creada correctamente.", documentId = pdfId });
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        return StatusCode(500, "Error al procesar la solicitud.");
    }
}

}
