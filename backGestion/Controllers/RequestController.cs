using Microsoft.AspNetCore.Mvc;
using backGestion.Services;
using backGestion.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace backGestion.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IPdfService _pdfService;

    public RequestController(IRequestService requestService, IPdfService pdfService)
    {
        _requestService = requestService;
        _pdfService = pdfService;
    }

    [HttpPost("with-document")]
    public async Task<IActionResult> PostRequestWithDocument(
        [FromForm] IFormFile file,
        [FromForm] string requestReason,
        [FromForm] string requestedBy,
        [FromForm] string malla,
        [FromForm] int semestre,
        [FromForm] string asignaturaCodigo,
        [FromForm] string asignaturaNombre)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Debe subir un documento PDF.");

        try
        {
            var pdfId = await _pdfService.UploadPdfAsync(file);

            var request = new Request
            {
                DocumentId = pdfId,
                RequestReason = requestReason,
                RequestedBy = requestedBy,
                Malla = malla,
                Semestre = semestre,
                AsignaturaCodigo = asignaturaCodigo,
                AsignaturaNombre = asignaturaNombre,
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _requestService.GetAllRequestsAsync();
        return Ok(requests);
    }

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveRequest(string id)
    {
        var result = await _requestService.UpdateStatusAsync(id, "Approved");
        return result ? Ok("Solicitud aprobada.") : NotFound("Solicitud no encontrada.");
    }

    [HttpPut("{id}/reject")]
    public async Task<IActionResult> RejectRequest(string id)
    {
        var result = await _requestService.UpdateStatusAsync(id, "Rejected");
        return result ? Ok("Solicitud rechazada.") : NotFound("Solicitud no encontrada.");
    }
}
