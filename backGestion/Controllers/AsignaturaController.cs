using backGestion.Interfaces;
using backGestion.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AsignaturaController : ControllerBase
{
    private readonly IAsignaturaService _asignaturaService;

    public AsignaturaController(IAsignaturaService asignaturaService)
    {
        _asignaturaService = asignaturaService;
    }

    [HttpGet]
public async Task<ActionResult<IEnumerable<Asignatura>>> GetAsignaturaMallaSemestre(
    [FromQuery] string malla_id, 
    [FromQuery] int semestre)
    {
        if (string.IsNullOrWhiteSpace(malla_id) || semestre <= 0)
        {
            return BadRequest("Debe proporcionar un malla_id válido y un semestre mayor a 0.");
        }

        var result = await _asignaturaService.GetAsignaturaMallaSemestre(malla_id, semestre);

        if (result == null || !result.Any())
        {
            return NotFound($"No se encontraron asignaturas para la malla '{malla_id}' en el semestre {semestre}.");
        }

        return Ok(result);
    }

}